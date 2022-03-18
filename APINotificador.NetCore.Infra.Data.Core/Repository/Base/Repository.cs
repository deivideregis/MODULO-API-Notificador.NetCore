using APINotificador.NetCore.Dominio.Core.Models;
using APINotificador.NetCore.Infra.Data.Core.Context;
using APINotificador.NetCore.Infra.Data.Core.Pagination;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Repository.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        public ContextBase Db { get; protected set; }
        public virtual DbSet<TEntity> DbSet { get; protected set; }

        protected PaginatedList<TEntity> _paginated { get; set; }
        protected async Task<PaginatedList<TEntity>> ReturnPaginatedList(IQueryable<TEntity> source, int? pageIndex = 1, int? pageSize = 25)
        {
            if (!pageIndex.HasValue || pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (!pageSize.HasValue || (pageSize < 1 || pageSize > 25))
            {
                pageSize = 25;
            }

            return await PaginatedList<TEntity>.CreateAsync(source, pageIndex.Value, pageSize.Value);
        }

        protected Repository(ContextBase context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public bool UpdateValeuWithViewModel(object model, object viewmodel)
        {
            bool hasColumnUpdate = false;

            var all_properties = viewmodel.GetType().GetProperties();

            foreach (var item in all_properties)
            {
                string propertyname = item.Name;

                //Thiago Nery: Verifica se a propriedade do view model existe no model antes de tentar imputar a informação
                if (!model.HasProperty(propertyname))
                    continue;

                object viewmodel_value = GetPropValue(viewmodel, propertyname);
                object model_value = GetPropValue(model, propertyname);

                if (viewmodel_value == null || string.Compare(propertyname, "Id") == 0 ||

                    //Thiago Nery: Se o campo for uma chave estrangeira do tipo guid, se o valor que tá vindo do view model, for vazio, não vai afetar o model.
                    (propertyname.ToLower().Contains("id") &&
                    Guid.Empty.ToString() == viewmodel_value.ToString() &&
                    model_value.ToString() != Guid.Empty.ToString()))

                    continue;

                if (!viewmodel_value.Equals(model_value))
                {
                    hasColumnUpdate = true;
                    SetValue(model, propertyname, viewmodel_value);
                    Db.Entry(model).Property(propertyname).IsModified = true;
                }
                else
                {
                    Db.Entry(model).Property(propertyname).IsModified = false;
                }
            }

            return hasColumnUpdate;
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            Type type = inputObject.GetType();

            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //Type propertyType = propertyInfo.PropertyType;
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

            propertyVal = Convert.ChangeType(propertyVal, targetType);

            propertyInfo.SetValue(inputObject, propertyVal, null);
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public virtual IQueryable<TEntity> ReturnIQueryable()
        {
            return Db.Set<TEntity>().AsQueryable().AsNoTracking();
        }

        public virtual async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await ReturnIQueryable().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            var model = await ReturnIQueryable().Where(d => d.Id.Equals(id)).FirstOrDefaultAsync();
            return model;
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await ReturnIQueryable().ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges();
        }

        public virtual async Task Remover(Expression<Func<TEntity, bool>> predicate)
        {
            var TEntityList = await Db.Set<TEntity>().AsQueryable().Where(predicate).ToListAsync();
            if (TEntityList != null)
                Db.Set<TEntity>().RemoveRange(TEntityList);

            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public virtual bool DominioExiste(Guid id)
        {
            return DbSet.Count(b => b.Id.Equals(id)) > 0;
        }

        public async Task Adicionar(List<TEntity> entity)
        {
            for (int i = 0; i < entity.Count(); i++)
            {
                TEntity addentity = entity[i];
                DbSet.Add(addentity);
                await SaveChanges();
            }
        }

        public async Task Atualizar(List<TEntity> entity)
        {
            for (int i = 0; i < entity.Count(); i++)
            {
                TEntity addentity = entity[i];
                DbSet.Update(addentity);
                await SaveChanges();
            }
        }
    }

    public static class RepositoryExtesion
    {
        public static string ToScape(this string str)
        {
            return string.Format("%{0}%", str);
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName.Replace(" DESC", "").Replace(" ASC", "")) != null;
        }

        public static IQueryable<T> OrderByNew<T>(this IQueryable<T> source, string ordering)
        {
            if (ordering == null || string.IsNullOrEmpty(ordering.Trim()))
            {
                return source;
            }

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            bool ascending = !ordering.Contains(" DESC");
            ordering = ordering.Replace(" DESC", "").Replace(" ASC", "");

            try
            {
                PropertyInfo property;
                Expression propertyAccess;
                if (ordering.Contains('.'))
                {
                    // support to be sorted on child fields.
                    String[] childProperties = ordering.Split('.');
                    property = type.GetProperty(childProperties[0]);
                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    for (int i = 1; i < childProperties.Length; i++)
                    {
                        property = property.PropertyType.GetProperty(childProperties[i]);
                        propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    }
                }
                else
                {
                    property = typeof(T).GetProperty(ordering);
                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                }
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                                                                 ascending ? "OrderBy" : "OrderByDescending",
                                                                 new[] { type, property.PropertyType }, source.Expression,
                                                                 Expression.Quote(orderByExp));
                //return  source.OrderBy(x => orderByExp);
                return source.Provider.CreateQuery<T>(resultExp);
            }
            catch
            {
                return source;
            }
        }

        private static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
