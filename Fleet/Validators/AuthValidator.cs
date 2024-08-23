using System;
using Fleet.Controllers.Model.Request.Auth;
using Fleet.Enums;
using Fleet.Interfaces.Repository;
using FluentValidation;

namespace Fleet.Validators;

public class AuthValidator :  AbstractValidator<EsqueceuSenhaRequest>
{
    public AuthValidator(IUsuarioRepository usuarioRepository)
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail invalido");

        RuleFor(x => x.Email).MustAsync(async (email, cancellationToken) => await usuarioRepository.ExisteEmail(email)).WithMessage("Email nao encontrado na base");
    }
}
