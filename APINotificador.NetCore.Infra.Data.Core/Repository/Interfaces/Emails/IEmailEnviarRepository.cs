using APINotificador.NetCore.Dominio.EmailRoot;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Emails
{
    public interface IEmailEnviarRepository : IRepository<Email>
    {
        Task<bool> EnviarEmail(Email model);
    }
}