using Bogus;
using CineAPI.Application.Movies.GetMovie;
using CineAPI.Application.Movies.UpdateMovie;
using CineAPI.Common.Dtos;
using CineAPI.Domain;
using CineAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class UpdateMovieHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private UpdateMovieHandler _handler = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _applicationDbContext = new ApplicationDbContext(dbContextOptions);
        _handler = new UpdateMovieHandler(_applicationDbContext);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_MovieUpdated()
    {
        // Arrange
        var movie = MovieFaker();
        _applicationDbContext.Movies.Add(movie);
        _applicationDbContext.SaveChanges();
        var command = new UpdateMovieCommand
        {
            MovieId = movie.MovieId,
            Title = "Titulo de prueba",
            Director = "Director de prueba"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        Assert.That(movie.Title, Is.EqualTo(command.Title));
        Assert.That(movie.Director, Is.EqualTo(command.Director));
    }

    private Movie MovieFaker()
    {
        return new Faker<Movie>()
            .RuleFor(x => x.MovieId, f => f.Random.Int(0))
            .RuleFor(x => x.Title, f => f.Name.FullName())
            .RuleFor(x => x.Duration, f => f.Random.Int(0));
    }
}
