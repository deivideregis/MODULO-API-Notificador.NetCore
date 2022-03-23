using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APINotificador.NetCore.Aplicacao.ViewModels.Emails
{
    public class EmailParaEnvioViewModel : BaseViewModelCadastro
    {

        [DisplayName("MAC")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string MACCorporativa { get; set; }

        [DisplayName("Assunto")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Assunto { get; set; }

        [DisplayName("Destinatario")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public List<string> ListaDestinatario { get; set; }

        [DisplayName("Corpo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(8000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Corpo { get; set; }

        [DisplayName("Anexo")]
        public List<IFormFile> Anexo { get; set; }
    }
}
