using APINotificador.NetCore.Dominio.RemetenteRoot;
using APINotificador.NetCore.Infra.Data.Core.TypeConfiguration.Remetentes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Context
{
    public class ContextBase : DbContext
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options) { }

        #region RemetenteCorporativa
        public DbSet<RemetenteCorporativa> RemetenteCorporativas { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Apply TypeConfiguration de domínios Remetente Corporativa
            modelBuilder.ApplyConfiguration(new RemetenteCorporativaTypeConfiguration());
            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextBase).Assembly);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
