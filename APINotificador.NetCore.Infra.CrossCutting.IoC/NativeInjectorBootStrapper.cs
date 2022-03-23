using APINotificador.NetCore.Aplicacao.Interfaces.EnvioEmail;
using APINotificador.NetCore.Aplicacao.Interfaces.Remetentes;
using APINotificador.NetCore.Aplicacao.Notificacoes;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.Services.Emails;
using APINotificador.NetCore.Aplicacao.Services.Remetentes;
using APINotificador.NetCore.Infra.Data.Core.Context;
using APINotificador.NetCore.Infra.Data.Core.Repository.Emails;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Emails;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Remetentes;
using APINotificador.NetCore.Infra.Data.Core.Repository.Remetentes;
using Microsoft.Extensions.DependencyInjection;

namespace APINotificador.NetCore.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();

            #region Contextos
            services.AddScoped<ContextBase>();
            #endregion

            #region Repositórios
            //Remetentes corportativa
            services.AddScoped<IRemetenteCorporativaRepository, RemetenteCorporativaRepository>();

            //Remetentes corportativa
            services.AddScoped<IEmailEnviarRepository, EmailEnviarRepository>();
            #endregion

            #region service
            //Remetente corporativa
            services.AddScoped<IRemetenteCorporativaService, RemetenteCorporativaService>();

            services.AddScoped<IEmailEnviarService, EmailEnviarService>();
            #endregion
        }
    }
}
