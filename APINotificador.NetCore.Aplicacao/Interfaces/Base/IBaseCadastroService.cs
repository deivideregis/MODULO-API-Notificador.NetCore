using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using APINotificador.NetCore.Dominio.Core.Models;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Aplicacao.Interfaces.Base
{
    public interface IBaseCadastroService<TModel, TViewModel, TViewModelAdicionar, TViewModelAtualizar, TValidator>
         where TModel : Entity, new()
         where TValidator : AbstractValidator<TModel>, new()
         where TViewModelAtualizar : BaseViewModelCadastro, new()
    {
        bool ValidarModel(TModel model);
        bool ValidarModelSemRelacionamentos(Guid id);
        TModel MapearDominio(TViewModelAdicionar viewmodel);
        Task<bool> Adicionar(TModel model);
        Task<bool> Atualizar(TViewModelAtualizar viewmodel);
        Task<bool> Remover(Guid id, TModel model);
    }
}
