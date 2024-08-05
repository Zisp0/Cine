using CineAPI.Infraestructure;
using CineAPI.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Application.Movies.DeleteMovie;

public record DeleteMovieCommand(int MovieId) : IRequest<bool>;

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator(ApplicationDbContext dbContext)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.MovieId)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyMovieId)
            .MustAsync(async (id, cancellation) => await dbContext.Movies.AnyAsync(movie => movie.MovieId == id, cancellation))
            .WithMessage(ValidationMessages.MovieNotFound);
    }
}
