using CineAPI.Common.Dtos;
using CineAPI.Infraestructure;
using CineAPI.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Movies.GetMovie;

public record GetMovieQuery (int MovieId) : IRequest<MovieDto>;

public class GetMovieQueryValidator : AbstractValidator<GetMovieQuery>
{
    public GetMovieQueryValidator(ApplicationDbContext dbContext)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.MovieId)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyMovieId)
            .MustAsync(async (id, cancellation) => await dbContext.Movies.AnyAsync(movie => movie.MovieId == id, cancellation))
            .WithMessage(ValidationMessages.MovieNotFound);
    }
}
