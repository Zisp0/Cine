using CineAPI.Application.Movies.AddMovie;
using CineAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class AddMovieHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private AddMovieHandler _handler = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _applicationDbContext = new ApplicationDbContext(dbContextOptions);
        _handler = new AddMovieHandler(_applicationDbContext);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_MovieCreated()
    {
        // Arrange
        var request = new AddMovieCommand
        {
            Title = "Pellícula de ejemplo",
            Duration = 120
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.GreaterThan(0));
    }
}
