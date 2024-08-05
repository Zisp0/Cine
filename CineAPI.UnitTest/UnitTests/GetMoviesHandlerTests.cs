using Bogus;
using CineAPI.Application.Movies.GetMovies;
using CineAPI.Common.Dtos;
using CineAPI.Domain;
using CineAPI.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.UnitTest.UnitTests;

[TestFixture]
public class GetMoviesHandlerTests
{
    private ApplicationDbContext _applicationDbContext = null!;
    private GetMoviesHandler _handler = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _applicationDbContext = new ApplicationDbContext(dbContextOptions);
        _handler = new GetMoviesHandler(_applicationDbContext);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _applicationDbContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task Handle_GetMovieList()
    {
        // Arrange
        var movie = MovieFaker(3);
        _applicationDbContext.Movies.AddRange(movie);
        _applicationDbContext.SaveChanges();

        // Act
        var result = await _handler.Handle(new GetMoviesQuery(), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.IsInstanceOf<IReadOnlyCollection<MovieDto>>(result);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Handle_GetEmptyList()
    {
        // Arrange
        _applicationDbContext.Movies.RemoveRange(_applicationDbContext.Movies);
        _applicationDbContext.SaveChanges();

        // Act
        var result = await _handler.Handle(new GetMoviesQuery(), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.IsInstanceOf<IReadOnlyCollection<MovieDto>>(result);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    private IReadOnlyCollection<Movie> MovieFaker(int n)
    {
        return new Faker<Movie>()
            .RuleFor(x => x.MovieId, f => f.Random.Int(0))
            .RuleFor(x => x.Title, f => f.Name.FullName())
            .RuleFor(x => x.Duration, f => f.Random.Int(0))
            .Generate(n);
    }
}
