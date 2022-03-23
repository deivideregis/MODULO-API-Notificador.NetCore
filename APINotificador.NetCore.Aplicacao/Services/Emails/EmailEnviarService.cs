using APINotificador.NetCore.Aplicacao.Interfaces.EnvioEmail;
using APINotificador.NetCore.Aplicacao.Notificacoes.Interfaces;
using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using APINotificador.NetCore.Aplicacao.ViewModels.Emails;
using APINotificador.NetCore.Dominio.EmailRoot;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Emails;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Remetentes;
using AutoMapper;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Aplicacao.Services.Emails
{
    public class EmailEnviarService : BaseService, IEmailEnviarService
    {
        private readonly IEmailEnviarRepository _repository;
        
        private readonly IRemetenteCorporativaRepository _remetenterepository;

        public EmailEnviarService(INotificador notificador,
                                          IEmailEnviarRepository repository,
                                          IRemetenteCorporativaRepository remetenterepository,
                                          IMapper mapper) : base(notificador)
        {
            _repository = repository;
            
            _remetenterepository = remetenterepository;
        }

        public async Task<bool> EnviarEmail(EmailEnviarViewModel viewmodel)
        {
            if (!_remetenterepository.DominioExistenteMAC(viewmodel.MACCorporativa))
            {
                Notificar(string.Format("Não foi possível enviar e-mail, pois MAC {0} não consta na nossa base de dados", viewmodel.MACCorporativa));

                return false;
            }

            var model = new Email();

            model.MACCorporativa = viewmodel.MACCorporativa;
            model.Assunto = viewmodel.Assunto;
            model.ListaDestinatario = viewmodel.ListaDestinatario;
            model.Corpo = viewmodel.Corpo;
            model.Anexo = viewmodel.Anexo;
            
            model.PortaRemetente = viewmodel.PortaRemetente;
            model.SslRemetente = viewmodel.SslRemetente;
            model.EUsarCredencial = viewmodel.EUsarCredencial;
            model.ServidorRemetente = viewmodel.ServidorRemetente;
            model.NomeCorporativa = viewmodel.NomeCorporativa;
            model.EmailCorporativa = viewmodel.EmailCorporativa;
            model.SenhaCorporativa = _remetenterepository.DescriptografarSenha(viewmodel.SenhaCorporativa);
            
            bool EEnviado = await _repository.EnviarEmail(model);

            if(!EEnviado)
            {
                Notificar(string.Format("Não foi possível enviar e-mail. Motivo: {0}", model.Corpo));
            }

            return EEnviado;
        }

        public void Dispose()
        {
            _repository?.Dispose();
            _remetenterepository?.Dispose();
        }
    }
}
