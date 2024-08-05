using Bogus;
using CineAPI.Application.Users.LoginUser;
using CineAPI.Common.Dtos;
using CineAPI.Domain;
using CineAPI.Infraestructure;
using CineAPI.Services;
using CineAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class LoginUserHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private IConfiguration _configuration = null!;
    private TokenService _tokenService = null!;
    private LoginUserHandler _handler = null!;

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
        _handler = new LoginUserHandler(_applicationDbContext, _tokenService);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_UserLogin()
    {
        // Arrange
        var password = "Abcd1234@@";
        var user = UserFaker(password);
        _applicationDbContext.Users.Add(user);
        _applicationDbContext.SaveChanges();
        var request = new LoginUserCommand
        {
            Email = user.Email,
            Password = password
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.IsInstanceOf<TokenDto>(result);
    }

    private User UserFaker(string password)
    {
        return new Faker<User>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => Encrypt.GetSHA256(password))
            .RuleFor(x => x.RegisteredDate, f => new DateTime());
    }
}
