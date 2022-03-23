using APINotificador.NetCore.Dominio.EmailRoot;
using FluentValidation;

namespace APINotificador.NetCore.Aplicacao.Validation.Emails
{
    public class EmailEnviarValidation : AbstractValidator<Email>
    {
        public EmailEnviarValidation()
        {
            RuleFor(c => c.PortaRemetente)
                .GreaterThan(0).WithMessage("O campo {PropertyName} precisa ser maior que {ComparisonValue}");

            RuleFor(c => c.ServidorRemetente)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Length(3, 150).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.NomeCorporativa)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Length(3, 150).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.MACCorporativa)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Length(10, 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.EmailCorporativa)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Length(10, 150).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.SenhaCorporativa)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Length(10, 150).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");
        }

    }
}
