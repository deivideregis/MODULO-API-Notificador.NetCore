using APINotificador.NetCore.Aplicacao.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace APINotificador.NetCore.WebAPI.Configuration
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
        }
    }
}
