using APINotificador.NetCore.Dominio.Core.Models;
using System;

namespace APINotificador.NetCore.Dominio.RemetenteRoot
{
    public class RemetenteCorporativa : Entity
    {
        public int PortaRemetente { get; set; }
        public bool SslRemetente { get; set; }
        public bool EUsarCredencial { get; set; }
        public string ServidorRemetente { get; set; }
        public string NomeCorporativa { get; set; }
        public string MACCorporativa { get; set; }
        public string EmailCorporativa { get; set; }
        public string SenhaCorporativa { get; set; }

        public RemetenteCorporativa()
        {
            DataCadastro = DateTime.Now;
            Ativo = true;
        }
    }
}
