using API.Dtos;
using API.Entities;
using API.Shared;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Utilities;
public static class SecurityUtils
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        ArgumentNullException.ThrowIfNull(password);

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
        }

        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        ArgumentNullException.ThrowIfNull(password);

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
        }

        if (passwordHash.Length != 64)
        {
            throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(passwordHash));
        }

        if (passwordSalt.Length != 128)
        {
            throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(passwordSalt));
        }

        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
            {
                return false;
            }
        }

        return true;
    }

    public static string GenerateJwtToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var secret = Environment.GetEnvironmentVariable("Jwt:Secret");
        var expiresAt = Environment.GetEnvironmentVariable("Jwt:ExpirationInMinutes");

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(expiresAt)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}