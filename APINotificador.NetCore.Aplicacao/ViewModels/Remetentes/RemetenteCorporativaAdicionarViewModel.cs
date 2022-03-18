using APINotificador.NetCore.Aplicacao.ViewModels.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APINotificador.NetCore.Aplicacao.ViewModels.Remetentes
{
    public class RemetenteCorporativaAdicionarViewModel : BaseViewModelCadastro
    {
        [DisplayName("porta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int PortaRemetente { get; set; }

        [DisplayName("SSL")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool SslRemetente { get; set; }

        [DisplayName("usar credencial")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool EUsarCredencial { get; set; }

        [DisplayName("Servidor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string ServidorRemetente { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string NomeCorporativa { get; set; }

        [DisplayName("MAC")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string MACCorporativa { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string EmailCorporativa { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string SenhaCorporativa { get; set; }
    }
}
