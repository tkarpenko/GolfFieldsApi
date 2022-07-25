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
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var Summaries = new string[] { "Hot" };

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


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
}

