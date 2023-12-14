using AssistenteVendas.Domain.Usuario.Entities;
using FluentValidation;

namespace AssistenteVendas.Service.Validators
{
    public class UsuarioValidator : AbstractValidator<UsuarioEntity>
    {
        public UsuarioValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é uma propriedade mandatória.")
                .NotNull().WithMessage("Nome é uma propriedade mandatória.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email é uma propriedade mandatória.")
                .NotNull().WithMessage("Email é uma propriedade mandatória.");

            //RuleFor(c => c.Password)
            //    .NotEmpty().WithMessage("Senha é uma propriedade mandatória.")
            //    .NotNull().WithMessage("Senha é uma propriedade mandatória.");
        }
    }
}
