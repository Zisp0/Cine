using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Movies.GetMovie;

public class GetMovieHandler : IRequestHandler<GetMovieQuery, MovieDto>
{
    private readonly ApplicationDbContext _dbContext;

    public GetMovieHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MovieDto> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.Where(movie => movie.MovieId == request.MovieId)
            .Select(movie => new MovieDto
            {
                Id = movie.MovieId,
                Title = movie.Title,
                Director = movie.Director,
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate,
                Duration = movie.Duration,
                Description = movie.Description,
                OriginalLanguage = movie.OriginalLanguage
            }).FirstAsync(cancellationToken);
    }
}
