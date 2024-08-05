using CineAPI.Utilities;
using FluentValidation;
using MediatR;

namespace CineAPI.Application.Movies.AddMovie;

public class AddMovieCommand : IRequest<int>
{
    public string Title { get; set; } = default!;
    public string? Director { get; set; }
    public string? Genre { get; set; }
    public string? ReleaseDate { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; }
    public string? OriginalLanguage { get; set; }
}

public class AddMovieCommandValidator : AbstractValidator<AddMovieCommand>
{
    public AddMovieCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyTitleMovie)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.MaxLength50)
            .WithName("título");

        When(command => !string.IsNullOrWhiteSpace(command.Director), () =>
        {
            RuleFor(command => command.Director)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.MaxLength50)
                .WithName("nombre del director");
        });

        When(command => !string.IsNullOrWhiteSpace(command.Genre), () =>
        {
            RuleFor(command => command.Genre)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.MaxLength50)
                .WithName("género");
        });

        When(command => !string.IsNullOrWhiteSpace(command.ReleaseDate), () =>
        {
            RuleFor(command => command.ReleaseDate)
                .Matches("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\\d{4}$")
                .WithMessage(ValidationMessages.ReleaseDateFormat);
        });

        RuleFor(command => command.Duration)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyDuration);

        When(command => !string.IsNullOrWhiteSpace(command.Description), () =>
        {
            RuleFor(command => command.Description)
                .MaximumLength(500)
                .WithMessage(ValidationMessages.MaxLengthDescription);
        });

        When(command => !string.IsNullOrWhiteSpace(command.OriginalLanguage), () =>
        {
            RuleFor(command => command.OriginalLanguage)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.MaxLength50)
                .WithName("idioma original");
        });
    }
}