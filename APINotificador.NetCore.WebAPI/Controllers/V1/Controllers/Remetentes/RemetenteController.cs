using APINotificador.NetCore.Aplicacao.Interfaces.Remetentes;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.ViewModels.Remetentes;
using APINotificador.NetCore.Dominio.Core;
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
    [Route("api/v{version:apiVersion}/remetente")]
    [ApiController]
    public class RemetenteController : BaseController
    {
        private readonly IRemetenteCorporativaRepository _repository;
        private readonly IRemetenteCorporativaService _service;
        private readonly IMapper _mapper;
        
        public RemetenteController(INotificador notificador,
                                 IRemetenteCorporativaRepository repository,
                                 IMapper mapper,
                                 IRemetenteCorporativaService service) : base(notificador)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// Pesquisar lista de remetente
        /// </summary>
        /// <param name="id">Id (guid).</param>
        /// <param name="nomeCorporativa">Nome do remetente corporativa.</param>
        /// <param name="emailCorporativa">E-mail do remetente corporativa.</param>
        /// <param name="ativo">Status do registro Ativo ou Desativado.</param>
        /// <param name="pagina">Página da lista de item</param>
        /// <param name="tamanhoPagina">Total de itens por página</param>
        /// <param name="ordem">Campo para ordenação: Descricao e DataCadastro. Adicione DESC após o nome do campo para ordem inversa.</param>
        /// <returns>Lista paginada de Empresas</returns>
        ///  <remarks>
        ///     
        ///  Permissão: aberta
        /// 
        ///  </remarks>
        /// <response code="200">Registro de remetente corporativa.</response>
        /// <response code="400">Não foi possível localizar o registro de remetente corporativa.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ListaPaginada<RemetenteCorporativaExibicaoViewModel>), 200)]
        public async Task<IActionResult> GetPorTodosOsFiltros([FromQuery] Guid? id,
            [FromQuery] string nomeCorporativa,
            [FromQuery] string emailCorporativa,
            [FromQuery] bool? ativo,
            [FromQuery] int? pagina,
            [FromQuery] int? tamanhoPagina,
            [FromQuery] string ordem)
        {
            var models = await _repository.ObterPorTodosFiltros(id, nomeCorporativa, emailCorporativa, ativo, pagina, tamanhoPagina, ordem);

            ListaPaginada<RemetenteCorporativaExibicaoViewModel> retorno = new ListaPaginada<RemetenteCorporativaExibicaoViewModel>(
                _mapper.Map<List<RemetenteCorporativaExibicaoViewModel>>(models.ListaRetorno),
                models.PaginaAtual,
                models.TotalPaginas,
                models.TamanhoPagina,
                models.TotalItens);


            if (models.TotalItens == 0)
            {
                NotificarErro("remetente não encontrado na base de dados");
                return CustomResponse();
            }

            return CustomResponse(retorno);
        }

        /// <summary>
        /// Retorna registro de remetente por Id (guid).
        /// </summary>
        /// <param name="id">Id (guid) do remetente corporativa.</param>
        /// <returns>Registro de remetente corporativa.</returns>
        ///  <remarks>
        ///     
        ///  Permissão: aberta
        /// 
        ///  </remarks>
        /// <response code="200">Registro de remetente corporativa ao Id.</response>
        /// <response code="400">Não foi possível localizar o registro de remetente corporativa.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RemetenteCorporativaExibicaoViewModel), 200)]
        [ProducesResponseType(typeof(BadRequestRetorno), 404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = _mapper.Map<RemetenteCorporativaExibicaoViewModel>(await _repository.ObterPorId(id));
            if (model == null) return NotFound();
            return CustomResponse(model);
        }

        /// <summary>
        /// Atualiza registro de remetente corporativa.
        /// </summary>
        /// <param name="id">Id (guid) do registro de remetente corporativa.</param>
        /// <param name="viewmodel">View Model de remetente corporativa.</param>
        /// <returns>Registro configuração de remetente corporativa atualizado</returns>
        /// <remarks>
        /// Exemplo de requisição
        /// 
        ///     PUT /api/v1/remetente/atualizar-remetente/00000000-0000-0000-0000-000000000000
        ///     {
        ///        "PortaRemetente": "587"
        ///        "SslRemetente": "true"
        ///        "EUsarCredencial": "true"
        ///        "ServidorRemetente": "smtp.gmail.com"
        ///        "NomeCorporativa": "API Logs"
        ///        "EmailCorporativa": "deividsantos@gmail.com"
        ///        "SenhaCorporativa": "@Asdf1234"
        ///     }
        ///     
        ///     Campos obrigatórios:
        ///        Id, PortaRemetente, SslRemetente, EUsarCredencial, ServidorRemetente, NomeCorporativa, EmailCorporativa e SenhaCorporativa
        ///     
        ///  Permissão: Administrador
        /// 
        ///  </remarks>
        /// <response code="200">Registro de Model de remetente corporativa.</response>
        /// <response code="400">Não foi possível atualizar o registro de remetente corporativa.</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessRetorno<RemetenteCorporativaExibicaoViewModel>), 200)]
        [ProducesResponseType(typeof(BadRequestRetorno), 400)]
        [HttpPut("atualiza-remetente/{id:guid}")]
        public async Task<IActionResult> AtualizarRemetente(Guid id, [FromBody] RemetenteCorporativaAtualizarViewModel viewmodel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if(!_repository.DominioExiste(id))
            {
                NotificarErro("Id do remetente não encontrado na base de dados");
                return CustomResponse();
            }

            viewmodel.Id = id;

            bool EAtualizado = await _service.AtualizarRemetente(viewmodel);

            if (EAtualizado)
            {
                var modelsretorno = _repository.ObterPorId(viewmodel.Id);
                var viewModelsRetorno = _mapper.Map<RemetenteCorporativaExibicaoViewModel>(modelsretorno);
                return CustomResponse(viewModelsRetorno);
            }

            return CustomResponse();
        }

        /// <summary>
        /// Exclui registro de remetente.
        /// </summary>
        /// <param name="id">Id (guid) do registro de remetente corporativa.</param>
        /// <returns>Retorno do sucesso da operação.</returns>
        ///  <remarks>
        /// Exemplo de requisição
        ///  
        ///     DELETE /api/v1/remetente/remover-remetente/00000000-0000-0000-0000-000000000000
        ///     {
        ///     }
        ///     
        ///  Permissão: aberta
        /// 
        ///  </remarks>
        /// <response code="200">Registro excluído com sucesso.</response>
        /// <response code="400">Não foi possível excluir o registro.</response>
        [AllowAnonymous]
        [HttpDelete("remover-remetente/{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestRetorno), 400)]
        public async Task<IActionResult> RemoverRemetente(Guid id)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            //bool ERemovido = await _service.ExcluirRemetente(id);

            //if (ERemovido)
            //{
            //    return CustomResponse("Removido com sucesso.");
            //}

            RemetenteCorporativaAtualizarViewModel viewmodel = new RemetenteCorporativaAtualizarViewModel();

            viewmodel.Id = id;
            viewmodel.Ativo = false;

            bool EInativado = await _service.AtualizarRemetente(viewmodel);

            if (EInativado)
            {
                return CustomResponse("Removido com sucesso.");
            }

            return CustomResponse();
        }
    }
}
