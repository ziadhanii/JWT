namespace JWT.Controllers;

[Authorize(Roles = "Admin")]
public class SecuredController : BaseApiController
{
    [HttpGet]
    public IActionResult GetData()
    {

        return Ok("Hello From Secured Controller");

    }



}
