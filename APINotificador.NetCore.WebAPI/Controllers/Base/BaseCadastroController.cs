using APINotificador.NetCore.Aplicacao.Interfaces.Base;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using APINotificador.NetCore.Dominio.Core.Models;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APINotificador.NetCore.WebAPI.Controllers.Base
{
    public abstract class BaseCadastroController<TModel, TViewModel, TViewModelAdicionar, TViewModelAtualizar, TValidator> : BaseController
        where TModel : Entity, new()
        where TViewModel : BaseViewModelCadastro, new()
        where TViewModelAtualizar : BaseViewModelCadastro, new()
        where TValidator : AbstractValidator<TModel>, new()
    {
        protected readonly IMapper _mapper;
        protected readonly IBaseCadastroService<TModel, TViewModel, TViewModelAdicionar, TViewModelAtualizar, TValidator> _appService;
        protected readonly IRepository<TModel> _repository;

        protected virtual string _NomeController { get; set; }
        protected virtual string _NomeCompletoController { get; set; }

        public BaseCadastroController(string nomeController, string nomeCompletoController,
                                      INotificador notificador,
                                      IMapper mapper,
                                      IBaseCadastroService<TModel, TViewModel, TViewModelAdicionar, TViewModelAtualizar, TValidator> appService,
                                      IRepository<TModel> repository) : base(notificador)
        {
            _repository = repository;
            _NomeController = nomeController;
            _NomeCompletoController = nomeCompletoController;
            _appService = appService;
            _mapper = mapper;
        }

        protected override void Dispose(bool disposing)
        {
            _repository?.Dispose();
        }

        public virtual async Task<IActionResult> Post([FromBody] TViewModelAdicionar viewmodel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            TModel model = _appService.MapearDominio(viewmodel);

            await _appService.Adicionar(model);

            if (!_notificador.TemNotificacao())
            {
                TModel modelRetorno = await _repository.ObterPorId(model.Id);
                TViewModel retorno = _mapper.Map<TViewModel>(modelRetorno);
                return CustomResponseAdd(string.Format("api/v1/{0}/{1}", _NomeController, retorno.Id), retorno);
            }

            return CustomResponse();
        }

        public virtual async Task<IActionResult> Put(Guid id, [FromBody] TViewModelAtualizar viewmodel)
        {
            if (id != viewmodel.Id)
            {
                NotificarErro("O ID informado não é o mesmo que foi passado na query");
                return CustomResponse(viewmodel);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _appService.Atualizar(viewmodel);

            TViewModel retorno = _mapper.Map<TViewModel>(_repository.Buscar(m => m.Id.Equals(viewmodel.Id)).Result.FirstOrDefault());

            if (!_notificador.TemNotificacao())
            {
                return CustomResponse(retorno);
            }

            return CustomResponse(retorno);
        }

        public virtual async Task<IActionResult> Delete(Guid id)
        {
            TModel model = _repository.Buscar(m => m.Id.Equals(id)).Result.FirstOrDefault();

            if (model == null)
                return NotFound();

            await _appService.Remover(id, model);

            if (!_notificador.TemNotificacao())
            {
                return CustomResponse(new { Mensagem = string.Format("{0} excluída com sucesso.", _NomeCompletoController) });
            }

            return CustomResponse();
        }

        public virtual async Task<IActionResult> Get(Guid id)
        {
            TModel model = _repository.Buscar(m => m.Id.Equals(id)).Result.FirstOrDefault();

            if (model == null)
                return NotFound();

            return CustomResponse(_mapper.Map<TViewModel>(model));
        }
    }
}
