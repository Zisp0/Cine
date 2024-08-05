using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using CineAPI.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Users.LoginUser;

public class LoginUserCommand : IRequest<TokenDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator(ApplicationDbContext dbContext) 
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyField)
            .EmailAddress()
            .WithMessage(ValidationMessages.ValidField)
            .MustAsync(async (email, cancellation) => await dbContext.Users.AnyAsync(user => user.Email == email, cancellation))
            .WithMessage("No se encuentra registrado un usuario con ese correo electrónico.")
            .WithName("un correo electrónico");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyField)
            .WithName("una contraseña");
    }

}
