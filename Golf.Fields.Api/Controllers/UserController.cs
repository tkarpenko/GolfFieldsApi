using Golf.Fields.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golf.Fields.Api;

[ApiController]
[ApiVersion(ApiConstants.VERSION)]
[Route("api/v{version:ApiVersion}/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    
    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Auth([FromBody] AuthModel model)
    {
        try
        {
            SecurityServices secSvc = new SecurityServices();
            var token = secSvc.Authenticate(model.Phone);

            if (token == null || string.IsNullOrWhiteSpace(token.BearerToken))
               return Unauthorized();

            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        }

    }

    [AllowAnonymous]
    [HttpGet("TestGet")]
    public IActionResult TestGet()
    {
        try
        {
            return Ok("token text");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        }

    }
}

