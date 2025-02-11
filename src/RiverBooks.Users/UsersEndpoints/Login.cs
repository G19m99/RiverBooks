

using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UsersEndpoints;

internal class Login : Endpoint<LoginRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public Login(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(req.Email);
        if (user == null) 
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        bool loginSuccess = await _userManager.CheckPasswordAsync(user, req.Password);
        if(!loginSuccess)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        string jwtSecret = Config["Auth:JwtSecret"]!;
        string token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = jwtSecret;
            o.ExpireAt = DateTime.UtcNow.AddDays(1);
            o.User["EmailAddress"] = user.Email!;
        });

        await SendOkAsync(token, ct);
    }
}
