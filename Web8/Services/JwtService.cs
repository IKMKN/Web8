using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web8.Interfaces;
using Web8.Models.Entities;

namespace Web8.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions options;
    public JwtService(IOptions<JwtOptions> options)
    {
        this.options = options.Value;
    }
    public string GenerateToken(User user)
    {
        Claim[] claims = [  new ("Id", user.Id.ToString()),
                            new ("Login", user.Login.ToString()),
                            new ("GroupId",user.UserGroupId.ToString())];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(options.ExpiresHours));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
