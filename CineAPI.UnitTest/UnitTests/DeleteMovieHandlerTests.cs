using Bogus;
using CineAPI.Application.Movies.DeleteMovie;
using CineAPI.Domain;
using CineAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class DeleteMovieHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private DeleteMovieHandler _handler = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _applicationDbContext = new ApplicationDbContext(dbContextOptions);
        _handler = new DeleteMovieHandler(_applicationDbContext);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_MovieDeleted()
    {
        // Arrange
        var movie = MovieFaker();
        _applicationDbContext.Movies.Add(movie);
        _applicationDbContext.SaveChanges();

        // Act
        var result = await _handler.Handle(new DeleteMovieCommand(movie.MovieId), CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
    }

    private Movie MovieFaker()
    {
        return new Faker<Movie>()
            .RuleFor(x => x.MovieId, f => f.Random.Int(0))
            .RuleFor(x => x.Title, f => f.Name.FullName())
            .RuleFor(x => x.Duration, f => f.Random.Int(0));
    }
}
