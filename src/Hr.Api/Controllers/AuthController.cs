using Hr.Api.Auth;
using Hr.Api.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hr.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthOptions _options;
    private readonly TokenService _tokens;

    public AuthController(IOptions<AuthOptions> options, TokenService tokens)
    {
        _options = options.Value;
        _tokens = tokens;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _options.Users.FirstOrDefault(u =>
            string.Equals(u.Username, request.Username, StringComparison.OrdinalIgnoreCase)
            && u.Password == request.Password);

        if (user is null)
            return Unauthorized(new ErrorResponse("Invalid username or password."));

        var (token, expiresAt) = _tokens.Create(user.Username, user.Role);

        return Ok(new LoginResponse(token, user.Username, user.Role, expiresAt));
    }
}
