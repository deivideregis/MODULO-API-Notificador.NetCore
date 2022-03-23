using APINotificador.NetCore.Aplicacao.ViewModels.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Aplicacao.Interfaces.EnvioEmail
{
    public interface IEmailEnviarService : IDisposable
    {
        Task<bool> EnviarEmail(EmailEnviarViewModel viewModel);
    }
}
