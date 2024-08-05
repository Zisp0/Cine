using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;
using CineAPI.Application.Users.RegisterUser;
using CineAPI.Application.Users.LoginUser;
using CineAPI.Common.Dtos;

namespace CineAPI.Controllers;

public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<TokenDto>> RegisterUserAsync([FromBody]RegisterUserCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<TokenDto>> LoginUserAsync([FromBody] LoginUserCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}
