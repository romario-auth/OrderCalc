using Microsoft.AspNetCore.Mvc;

namespace OrderCalc.API.Controllers;

[Route("api/v1/Tools")]
[ApiController]
public class ToolsController : ControllerBase
{
    [HttpGet, Route("ping")]
    public string Ping()
    {
        return "Pong";
    }
}