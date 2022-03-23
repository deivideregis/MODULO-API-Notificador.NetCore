using APINotificador.NetCore.Dominio.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace APINotificador.NetCore.Dominio.EmailRoot
{
    public class Email : Entity
    {
        public int PortaRemetente { get; set; }
        public bool SslRemetente { get; set; }
        public bool EUsarCredencial { get; set; }
        public string ServidorRemetente { get; set; }
        public string NomeCorporativa { get; set; }
        public string MACCorporativa { get; set; }
        public string EmailCorporativa { get; set; }
        public string SenhaCorporativa { get; set; }
        public string Assunto { get; set; }
        public List<string> ListaDestinatario { get; set; }
        public string Corpo { get; set; }
        public List<IFormFile> Anexo { get; set; }

        public Email()
        {
            DataCadastro = DateTime.Now;
            Ativo = true;
        }
    }
}
