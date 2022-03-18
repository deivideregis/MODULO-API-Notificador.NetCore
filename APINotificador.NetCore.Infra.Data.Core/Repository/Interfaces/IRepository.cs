using APINotificador.NetCore.Dominio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);
        Task Adicionar(List<TEntity> entity);
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Atualizar(List<TEntity> entity);
        Task Remover(Guid id);
        Task Remover(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();
        bool DominioExiste(Guid id);
        bool UpdateValeuWithViewModel(object model, object viewmodel);
    }
}
