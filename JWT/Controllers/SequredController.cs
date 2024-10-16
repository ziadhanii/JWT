namespace JWT.Controllers;
[Authorize]
public class SequredController : BaseApiController
{
    [HttpGet]
    public IActionResult GetData()
    {

        return Ok("Hello From Sequred Controller");

    }



}
