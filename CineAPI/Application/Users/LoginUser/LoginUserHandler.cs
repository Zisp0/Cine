using CineAPI.Common;
using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using CineAPI.Services;
using CineAPI.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Users.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, TokenDto>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TokenService _tokenService;

    public LoginUserHandler(ApplicationDbContext dbContext, TokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == command.Email && user.Password == Encrypt.GetSHA256(command.Password));
        if (user == null)
        {
            throw new CineException("La contraseña ingresada no es la correcta.");
        }
        return new TokenDto { Token = _tokenService.GenerateToken(user.UserId) };
    }
}
