using Bogus;
using CineAPI.Application.Movies.GetMovie;
using CineAPI.Common.Dtos;
using CineAPI.Domain;
using CineAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class GetMovieHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private GetMovieHandler _handler = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _applicationDbContext = new ApplicationDbContext(dbContextOptions);
        _handler = new GetMovieHandler(_applicationDbContext);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_GetMovie()
    {
        // Arrange
        var movie = MovieFaker();
        _applicationDbContext.Movies.Add(movie);
        _applicationDbContext.SaveChanges();

        // Act
        var result = await _handler.Handle(new GetMovieQuery(movie.MovieId), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.IsInstanceOf<MovieDto>(result);
    }

    private Movie MovieFaker()
    {
        return new Faker<Movie>()
            .RuleFor(x => x.MovieId, f => f.Random.Int(0))
            .RuleFor(x => x.Title, f => f.Name.FullName())
            .RuleFor(x => x.Duration, f => f.Random.Int(0));
    }
}
