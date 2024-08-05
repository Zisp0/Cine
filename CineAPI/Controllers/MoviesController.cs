using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;
using CineAPI.Common.Dtos;
using CineAPI.Application.Movies.GetMovies;
using CineAPI.Application.Movies.AddMovie;
using Microsoft.AspNetCore.Authorization;
using CineAPI.Application.Movies.UpdateMovie;
using CineAPI.Infraestructure;
using CineAPI.Application.Movies.DeleteMovie;
using CineAPI.Application.Movies.GetMovie;

namespace CineAPI.Controllers;

public class MoviesController : BaseController
{
    private readonly IMediator _mediator;
    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<MovieDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<MovieDto>>> GetMoviesAsync()
    {
        return Ok(await _mediator.Send(new GetMoviesQuery()));
    }

    [Authorize]
    [HttpGet("{movieId}")]
    [ProducesResponseType(typeof(MovieDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<MovieDto>> GetMovieAsync(int movieId)
    {
        return Ok(await _mediator.Send(new GetMovieQuery(movieId)));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> AddMovieAsync([FromBody] AddMovieCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize]
    [HttpPut("{movieId}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateMovieAsync(int movieId, [FromBody] UpdateMovieCommand command)
    {
        var userId = HttpContext.GetUserIdFromHttpContext();
        command.UserId = userId;
        command.MovieId = movieId;
        return Ok(await _mediator.Send(command));
    }

    [Authorize]
    [HttpDelete("{movieId}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteMovieAsync(int movieId)
    {
        return Ok(await _mediator.Send(new DeleteMovieCommand(movieId)));
    }
}
