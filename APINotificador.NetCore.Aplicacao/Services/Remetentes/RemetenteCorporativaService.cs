using APINotificador.NetCore.Aplicacao.Interfaces.Remetentes;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using APINotificador.NetCore.Aplicacao.ViewModels.Remetentes;
using APINotificador.NetCore.Dominio.RemetenteRoot;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Remetentes;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Aplicacao.Services.Remetentes
{
    public class RemetenteCorporativaService : BaseService, IRemetenteCorporativaService
    {
        private readonly IRemetenteCorporativaRepository _repository;

        public RemetenteCorporativaService(INotificador notificador,
                                          IRemetenteCorporativaRepository repository,
                                          IMapper mapper) : base(notificador)
        {
            _repository = repository;
        }

        public async Task<RemetenteCorporativa> RegistrarNovaRemetente(RemetenteCorporativaAdicionarViewModel viewModel)
        {
            return await RegistrarRemetente(viewModel);
        }

        private async Task<RemetenteCorporativa> RegistrarRemetente(RemetenteCorporativaAdicionarViewModel viewmodel)
        {
            string inicial = string.Empty;

            if (_repository.DominioExistente(viewmodel.EmailCorporativa))
            {
                Notificar(string.Format("Não é possível cadastrar remetente, pois e-mail {0} já consta na nossa base de dados", viewmodel.EmailCorporativa));
            }

            //if (_repository.DominioExistenteMAC(viewmodel.MACCorporativa))
            //{
            //    Notificar(string.Format("Não é possível cadastrar remetente, pois MAC {0} já consta na nossa base de dados", viewmodel.MACCorporativa));
            //}

            if (_notificador.TemNotificacao())
            {
                return null;
            }

            RemetenteCorporativa modelRemetente = new RemetenteCorporativa();

            modelRemetente.PortaRemetente = viewmodel.PortaRemetente;
            modelRemetente.SslRemetente = viewmodel.SslRemetente;
            modelRemetente.EUsarCredencial = viewmodel.EUsarCredencial;
            modelRemetente.ServidorRemetente = viewmodel.ServidorRemetente;
            modelRemetente.NomeCorporativa = viewmodel.NomeCorporativa;
            modelRemetente.MACCorporativa = _repository.GetEnderecoMAC();
            modelRemetente.EmailCorporativa = viewmodel.EmailCorporativa;
            modelRemetente.SenhaCorporativa = _repository.CriptografarSenha(viewmodel.SenhaCorporativa);

            await _repository.Adicionar(modelRemetente);

            var retorno = await _repository.ObterPorId(modelRemetente.Id);

            if (retorno == null)
            {
                Notificar("Não foi possível adicionar o remetente.");
                return null;
            }

            modelRemetente = null;

            return retorno;
        }

        public virtual bool MapearAtualizacoesRemetente(RemetenteCorporativa model, RemetenteCorporativaAtualizarViewModel viewmodel)
        {
            return _repository.UpdateValeuWithViewModel(model, viewmodel);
        }

        public async Task<bool> AtualizarRemetente(RemetenteCorporativaAtualizarViewModel viewmodel)
        {
            if (!_repository.DominioExiste(viewmodel.Id))
            {
                Notificar("remetente não encontrada.");
                return false;
            }

            RemetenteCorporativa model = _repository.RetornaRemetentePorId(viewmodel.Id);

            bool temAtualizacao = MapearAtualizacoesRemetente(model, viewmodel);

            model.SenhaCorporativa = _repository.CriptografarSenha(viewmodel.SenhaCorporativa);

            if (!temAtualizacao)
            {
                Notificar("Não há alterações no registro de {0}.", "Remetente");
                return false;
            }

            await _repository.Atualizar(model);
            return true;
        }

        public async Task<bool> ExcluirRemetente(Guid idremetente)
        {
            try
            {
                var remetenteInativar = await _repository.ObterPorTodosFiltros(null, null, null, null, null, null, null);

                remetenteInativar = null;

                await _repository.Remover(p => p.Id == idremetente);

                return true;
            }
            catch (Exception ex)
            {
                Notificar(string.Format("Não foi possível remover remetente. Motivo: {0}", ex.Message));
                return false;
            }
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
