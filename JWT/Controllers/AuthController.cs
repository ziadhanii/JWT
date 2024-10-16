﻿namespace JWT.Controllers;

public class AuthController(IAuthService authService) : BaseApiController
{

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
    {
        var result = await authService.GetTokenAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        if (!string.IsNullOrEmpty(result.RefreshToken))
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    [HttpPost("add-to-role")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.AddRoleAsync(model);

        if (!string.IsNullOrEmpty(result))
            return BadRequest(result);

        return Ok(model);
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await authService.RefreshTokenAsync(refreshToken);

        if (!result.IsAuthenticated)
            return BadRequest(result);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }




    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
    {
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest("Token is required!");

        var result = await authService.RevokeTokenAsync(token);

        if (!result)
            return BadRequest("Token is invalid!");

        return Ok();
    }


    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}