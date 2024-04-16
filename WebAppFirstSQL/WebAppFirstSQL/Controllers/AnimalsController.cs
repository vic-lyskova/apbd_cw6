using Microsoft.AspNetCore.Mvc;

namespace WebAppFirstSQL.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAnimals()
    {
        return Ok();
    }
}