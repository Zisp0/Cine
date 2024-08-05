using CineAPI.Application.Users.RegisterUser;
using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using CineAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class RegisterUserHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private IConfiguration _configuration = null!;
    private TokenService _tokenService= null!;
    private RegisterUserHandler _handler = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _configuration = Substitute.For<IConfiguration>();
        _configuration["Jwt:Key"].Returns("CYcUG-awyQZ6-M5znV8F-4fuXN3-AtRg");
        _configuration["Jwt:DurationMinutes"].Returns("20");
        _tokenService = new TokenService(_configuration);
        _applicationDbContext = new ApplicationDbContext(dbContextOptions);
        _handler = new RegisterUserHandler(_applicationDbContext, _tokenService);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_UserRegistered()
    {
        // Arrange
        var request = new RegisterUserCommand
        {
            Email = "ejemplo@registro.com",
            Password = "Abcd1234@@"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.IsInstanceOf<TokenDto>(result);
    }
}
