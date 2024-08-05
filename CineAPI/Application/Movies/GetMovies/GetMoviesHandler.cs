using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Movies.GetMovies;

public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, IReadOnlyCollection<MovieDto>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetMoviesHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.Select(movie => new MovieDto
        {
            Id = movie.MovieId,
            Title = movie.Title,
            Director = movie.Director,
            Genre = movie.Genre,
            ReleaseDate = movie.ReleaseDate,
            Duration = movie.Duration,
            Description = movie.Description,
            OriginalLanguage = movie.OriginalLanguage
        }).ToListAsync(cancellationToken);
    }
}
