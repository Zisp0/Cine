using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BaseController : ControllerBase
{
}
