namespace JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (ModelState.IsValid == false) return BadRequest(ModelState);
        var result = await authService.RegisterAsync(model);

        if (result.IsAuthenticated == false) return BadRequest(result.Message);

        return Ok(result);

    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
    {
        if (ModelState.IsValid == false) return BadRequest(ModelState);

        var result = await authService.GetTokenAsync(model);

        if (result.IsAuthenticated == false) return BadRequest(result.Message);

        return Ok(result);

    }





}
