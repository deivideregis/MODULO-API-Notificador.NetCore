using APINotificador.NetCore.Aplicacao.Interfaces.Base;
using APINotificador.NetCore.Aplicacao.Validation.Remetentes;
using APINotificador.NetCore.Aplicacao.ViewModels.Remetentes;
using APINotificador.NetCore.Dominio.RemetenteRoot;
using System;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Aplicacao.Interfaces.Remetentes
{
    public interface IRemetenteCorporativaService : IDisposable
    {
        Task<RemetenteCorporativa> RegistrarNovaRemetente(RemetenteCorporativaAdicionarViewModel viewModel);

        Task<bool> AtualizarRemetente(RemetenteCorporativaAtualizarViewModel viewmodel);

        Task<bool> ExcluirRemetente(Guid idremetente);
    }
}
