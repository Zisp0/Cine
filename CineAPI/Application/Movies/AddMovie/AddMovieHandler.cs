using CineAPI.Domain;
using CineAPI.Infraestructure;
using MediatR;

namespace CineAPI.Application.Movies.AddMovie;

public class AddMovieHandler : IRequestHandler<AddMovieCommand, int>
{
    private readonly ApplicationDbContext _dbContext;

    public AddMovieHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(AddMovieCommand command, CancellationToken cancellation)
    {
        var movie = new Movie
        {
            Title = command.Title,
            Director = command.Director,
            Genre = command.Genre,
            ReleaseDate = command.ReleaseDate != null ? command.ReleaseDate.ParseDate() : null,
            Duration = command.Duration,
            Description = command.Description,
            OriginalLanguage = command.OriginalLanguage
        };

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellation);
        return movie.MovieId;
    }
}
