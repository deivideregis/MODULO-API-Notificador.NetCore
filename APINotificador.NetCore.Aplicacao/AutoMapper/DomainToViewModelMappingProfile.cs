using APINotificador.NetCore.Aplicacao.ViewModels.Remetentes;
using APINotificador.NetCore.Dominio.RemetenteRoot;
using AutoMapper;
using System;
using System.Globalization;

namespace APINotificador.NetCore.Aplicacao.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            //Remetente corporativa - acesso - adicionar
            CreateMap<RemetenteCorporativa, RemetenteCorporativaAdicionarViewModel>().ReverseMap();
            //Remetente corporativa - acesso - exibir
            CreateMap<RemetenteCorporativa, RemetenteCorporativaExibicaoViewModel>().ReverseMap();
                //.IncludeAllDerived()
                //.ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(scr => DateTime.ParseExact(scr.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("pt-BR"))))
                //.ReverseMap();
        }
    }
}
