using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using System;

namespace APINotificador.NetCore.Aplicacao.ViewModels.Remetentes
{
    public class RemetenteCorporativaExibicaoViewModel : BaseViewModelCadastro
    {
        public override Guid Id { get; set; }
        public int PortaRemetente { get; set; }
        public bool SslRemetente { get; set; }
        public bool EUsarCredencial { get; set; }
        public string ServidorRemetente { get; set; }
        public string NomeCorporativa { get; set; }
        public string MACCorporativa { get; set; }
        public string EmailCorporativa { get; set; }
    }
}
