using CineAPI.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Movies.UpdateMovie;

public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateMovieHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FirstAsync(movie => movie.MovieId == command.MovieId);
        movie.Title = command.Title;
        movie.Director = command.Director;
        movie.Genre = command.Genre;
        movie.ReleaseDate = command.ReleaseDate != null ? command.ReleaseDate.ParseDate() : null;
        movie.Duration = command.Duration;
        movie.Description = command.Description;
        movie.OriginalLanguage = command.OriginalLanguage;
        movie.UpdatedBy = command.UserId;
        movie.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}
