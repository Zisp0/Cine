using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using CineAPI.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Users.RegisterUser;

public class RegisterUserCommand : IRequest<TokenDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(ApplicationDbContext dbContext) 
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyField)
            .EmailAddress()
            .WithMessage(ValidationMessages.ValidField)
            .MustAsync(async (email, cancellation) => !await dbContext.Users.AnyAsync(user => user.Email == email, cancellation))
            .WithMessage("Ya se encuentra registrado un usuario con ese correo electrónico.")
            .WithName("un correo electrónico");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyField)
            .MinimumLength(10)
            .WithMessage("La contraseña debe tener al menos 10 caracteres.")
            .Matches(@"[a-z]")
            .WithMessage("La contraseña debe contener al menos una letra minúscula.")
            .Matches(@"[A-Z]")
            .WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches(@"[!@#?\]]")
            .WithMessage("La contraseña debe contener al menos uno de los siguientes caracteres especiales: !, @, #, ?, ]")
            .WithName("una contraseña");
    }

}
