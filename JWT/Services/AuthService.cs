
namespace JWT.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<JWT.Helpers.JWT> _jwt;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT.Helpers.JWT> jwt)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwt = jwt;
    }

    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Message = "Email is already registered" };

        if (await _userManager.FindByNameAsync(model.Username) is not null)
            return new AuthModel { Message = "Username is already registered" };

        var user = new ApplicationUser
        {
            UserName = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded == false)
        {
            var errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += $"{error.Description},";
            }
            return new AuthModel { Message = errors.TrimEnd(',') };
        }

        await _userManager.AddToRoleAsync(user, "User");
        var jwtSecurityToken = await CreateJwtToken(user);
        return new AuthModel
        {
            Email = user.Email,
            IsAuthenticated = true,
            Roles = new List<string> { "User" },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName
        };
    }

    public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
    {
        var authModel = new AuthModel();

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null || await _userManager.CheckPasswordAsync(user, model.Password) == false)
        {
            authModel.Message = "Email or Password is incorrect";
            return authModel;
        }

        var jwtSecurityToken = await CreateJwtToken(user);
        var rolesList = await _userManager.GetRolesAsync(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.Email = model.Email;
        authModel.Username = user.UserName;
        authModel.Roles = rolesList.ToList();
        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authModel.RefreshToken = activeRefreshToken.Token;
            authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
        }


        return authModel;
    }

    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user is null || await _roleManager.RoleExistsAsync(model.Role) == false)
            return "Invalid User ID or Role";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User is already assigned to this role";

        var result = await _userManager.AddToRoleAsync(user, model.Role);
        return result.Succeeded ? string.Empty : "Something went wrong";
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Value.Issuer,
            audience: _jwt.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.Value.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };

    }
}
