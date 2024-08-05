using CineAPI.Common.Dtos;
using MediatR;

namespace CineAPI.Application.Movies.GetMovies;

public record GetMoviesQuery : IRequest<IReadOnlyCollection<MovieDto>>;
