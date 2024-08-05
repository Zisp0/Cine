using CineAPI.Common.Dtos;
using CineAPI.Domain;
using CineAPI.Infraestructure;
using CineAPI.Services;
using CineAPI.Utilities;
using MediatR;

namespace CineAPI.Application.Users.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, TokenDto>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TokenService _tokenService;

    public RegisterUserHandler(ApplicationDbContext dbContext, TokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<TokenDto> Handle(RegisterUserCommand command, CancellationToken cancellation)
    {
        var user = new User
        {
            Email = command.Email,
            Password = Encrypt.GetSHA256(command.Password),
            RegisteredDate = DateTime.Now
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellation);
        return new TokenDto { Token = _tokenService.GenerateToken(user.UserId) };
    }
}
