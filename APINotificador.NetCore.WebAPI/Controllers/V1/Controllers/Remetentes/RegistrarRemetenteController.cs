using APINotificador.NetCore.Aplicacao.Interfaces.Remetentes;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.Validation.Remetentes;
using APINotificador.NetCore.Aplicacao.ViewModels.Remetentes;
using APINotificador.NetCore.Dominio.Core;
using APINotificador.NetCore.Dominio.RemetenteRoot;
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
    [Route("api/v{version:apiVersion}/registrar")]
    [ApiController]
    public class RegistrarRemetenteController : BaseController
    {
        private readonly IRemetenteCorporativaRepository _repository;
        private readonly IRemetenteCorporativaService _service;
        private readonly IMapper _mapper;

        public RegistrarRemetenteController(INotificador notificador,
                                                 IRemetenteCorporativaService appservice,
                                                 IRemetenteCorporativaRepository repository,
                                                 IMapper mapper,
                                                 IRemetenteCorporativaService service) : base(notificador)
        {
            _repository = repository;
            _service = service;

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
        [HttpPost("novo-remetente")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarNovoRemetente([FromBody] RemetenteCorporativaAdicionarViewModel viewmodel)
        {
            if (viewmodel == null)
            {
                NotificarErro("Informações do remetente json inválida.");
                return CustomResponse(viewmodel);
            }

            ModelState.Remove("MACCorporativa");

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);
            
            var model = new RemetenteCorporativa();

            model = await _service.RegistrarNovaRemetente(viewmodel);

            return CustomResponse(_mapper.Map<RemetenteCorporativaExibicaoViewModel>(model));
        }
    }
}
