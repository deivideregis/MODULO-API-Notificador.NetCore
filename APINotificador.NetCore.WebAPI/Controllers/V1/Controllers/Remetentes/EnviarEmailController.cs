using APINotificador.NetCore.Aplicacao.Interfaces.EnvioEmail;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.ViewModels.Emails;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Remetentes;
using APINotificador.NetCore.WebAPI.Controllers.Base;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APINotificador.NetCore.WebAPI.Controllers.V1.Controllers.Remetentes
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/email")]
    [ApiController]
    public class EnviarEmailController : BaseController
    {
        private readonly IEmailEnviarService _service;

        private readonly IRemetenteCorporativaRepository _RemetenteRepository;

        private readonly IMapper _mapper;

        public EnviarEmailController(INotificador notificador,
                                                 IEmailEnviarService service,
                                                 IRemetenteCorporativaRepository RemetenteRepository,
                                                 IMapper mapper) : base(notificador)
        {
            _service = service;

            _RemetenteRepository = RemetenteRepository;

            _mapper = mapper;
        }

        /// <summary>
        /// Cadastra novo remetente.
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <returns>Resultado da operação.</returns>
        /// <remarks>
        /// Exemplo de requisição
        /// 
        ///     POST /api/v1/remetente/novo-remetente
        ///     {
        ///        "PortaRemetente": "587",
        ///        "SslRemetente": "true",
        ///        "EUsarCredencial": "true",
        ///        "ServidorRemetente": "smtp.gmail.com",
        ///        "NomeCorporativa": "API Logs",
        ///        "EmailCorporativa": "deividsantos@gmail.com",
        ///        "SenhaCorporativa": "@Asdf1234"
        ///     }
        ///     
        ///     Campos obrigatórios:
        ///        Descricao: O tamanho do campo Descricao deve ser de 3 a 50 caracteres
        ///
        ///  Permissão: aberto
        ///  
        /// </remarks>
        /// <response code="201">Novo registro de remetente corporativa criado.</response>
        /// <response code="400">Não foi possível criar o registro de remetente corporativa.</response>
        [HttpPost("enviar")]
        [AllowAnonymous]
        public async Task<IActionResult> EnviarEmail([FromForm] EmailParaEnvioViewModel viewmodel)
        {
            if (viewmodel == null)
            {
                NotificarErro("Informações do envio de e-mail json inválida.");
                return CustomResponse(viewmodel);
            }

            var DadosRementente = _RemetenteRepository.RetornaRemetentePorMac(viewmodel.MACCorporativa);

            if (DadosRementente == null)
            {
                NotificarErro(string.Format("Não foi encontrado os dados remetentes do MAC '{0}' na base de dados", viewmodel.MACCorporativa));
                return CustomResponse(viewmodel);
            }

            EmailEnviarViewModel viewmodelemail = new EmailEnviarViewModel();

            //viewmodel.Id = new Guid();
            //viewmodel.DataCadastro = DateTime.Now;
            //viewmodel.Ativo = true;

            viewmodelemail.MACCorporativa = viewmodel.MACCorporativa;
            viewmodelemail.Assunto = viewmodel.Assunto;
            viewmodelemail.ListaDestinatario = viewmodel.ListaDestinatario;
            viewmodelemail.Corpo = viewmodel.Corpo;
            viewmodelemail.Anexo = viewmodel.Anexo;


            viewmodelemail.PortaRemetente = DadosRementente.PortaRemetente;
            viewmodelemail.SslRemetente = DadosRementente.SslRemetente;
            viewmodelemail.EUsarCredencial = DadosRementente.EUsarCredencial;
            viewmodelemail.ServidorRemetente = DadosRementente.ServidorRemetente;
            viewmodelemail.NomeCorporativa = DadosRementente.NomeCorporativa;
            viewmodelemail.EmailCorporativa = DadosRementente.EmailCorporativa;
            viewmodelemail.SenhaCorporativa = DadosRementente.SenhaCorporativa;


            //if (!ModelState.IsValid)
            //    return CustomResponse(ModelState);

            await _service.EnviarEmail(viewmodelemail);

            return CustomResponse();
        }
    }
}
