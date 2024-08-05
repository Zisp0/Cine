using CineAPI.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Movies.DeleteMovie;

public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteMovieHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FirstAsync(movie => movie.MovieId == request.MovieId);
        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
