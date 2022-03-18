using APINotificador.NetCore.Aplicacao.Interfaces.Base;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using APINotificador.NetCore.Dominio.Core.Models;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces;
using AutoMapper;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Aplicacao.Services.Base
{
    public abstract class BaseCadastroService<TModel, TViewModel, TViewModelAdicionar, TViewModelAtualizar, TValidator>
        : BaseService, IBaseCadastroService<TModel, TViewModel, TViewModelAdicionar, TViewModelAtualizar, TValidator>
        where TModel : Entity, new()
        where TValidator : AbstractValidator<TModel>, new()
        where TViewModelAtualizar : BaseViewModelCadastro, new()
    {
        protected readonly IRepository<TModel> _repository;
        protected readonly IMapper _mapper;

        protected string _nomeDominio { get; private set; }

        protected BaseCadastroService(INotificador notificador,
                                      IRepository<TModel> repository,
                                      IMapper mapper,
                                      string nomeDominio) : base(notificador)
        {
            _repository = repository;
            _mapper = mapper;
            _nomeDominio = nomeDominio;
        }

        public async Task<bool> Adicionar(TModel model)
        {
            if (!ValidarModel(model))
                return false;

            if (!ValidarAdicionarModel(model))
                return false;

            try
            {
                await _repository.Adicionar(model);
            }
            catch (Exception ex)
            {
                Notificar("Não é possível adicionar {0}. Motivo: {1}.", _nomeDominio, ex.InnerException.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> Atualizar(TViewModelAtualizar viewmodel)
        {
            if (!_repository.DominioExiste(viewmodel.Id))
            {
                Notificar("{0} não existe.", _nomeDominio);
                return false;
            }

            TModel model = await _repository.ObterPorId(viewmodel.Id);

            bool temAtualizacao = MapearAtualizacoes(model, viewmodel);

            if (!temAtualizacao)
            {
                Notificar("Não há alterações no registro de {0}.", _nomeDominio);
                return false;
            }

            if (!ValidarModel(model))
                return false;

            try
            {
                await _repository.Atualizar(model);
                return true;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível atualizar o registro de {0}. Motivo: {1}", _nomeDominio, ex.InnerException);
                return false;
            }
        }

        public async Task<bool> Remover(Guid id, TModel model)
        {
            if (!ValidarExclusao(model))
            {
                return false;
            }

            if (!ValidarModelSemRelacionamentos(id))
            {
                return false;
            }

            try
            {
                await _repository.Remover(id);
                return true;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível excluir o {0}. Motivo: {1}", _nomeDominio, ex.InnerException.Message);
                return false;
            }
        }

        public virtual TModel MapearDominio(TViewModelAdicionar viewmodel)
        {
            TModel model = _mapper.Map<TModel>(viewmodel);
            return model;
        }

        public virtual bool ValidarModel(TModel model)
        {
            if (!ExecutarValidacao(new TValidator(), model))
                return false;
            return true;
        }

        public virtual bool ValidarModelSemRelacionamentos(Guid id)
        {
            return true;
        }

        public virtual bool ValidarAdicionarModel(TModel model)
        {
            return true;
        }

        public virtual bool ValidarExclusao(TModel model)
        {
            return true;
        }

        public virtual bool MapearAtualizacoes(TModel model, TViewModelAtualizar viewmodel)
        {
            return _repository.UpdateValeuWithViewModel(model, viewmodel);
        }
    }
}
