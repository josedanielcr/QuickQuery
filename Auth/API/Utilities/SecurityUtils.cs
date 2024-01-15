using API.Configuration;
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
        ValidatePasswordForHashing(password);
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static void ValidatePasswordForHashing(string password)
    {
        ArgumentNullException.ThrowIfNull(password);

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
        }
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        ValidateLoginCredentials(password, passwordHash, passwordSalt);

        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return CompareHashes(passwordHash, computedHash);
    }

    private static bool CompareHashes(byte[] passwordHash, byte[] computedHash)
    {
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
            {
                return false;
            }
        }
        return true;
    }

    private static void ValidateLoginCredentials(string password, byte[] passwordHash, byte[] passwordSalt)
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
    }

    public static string GenerateJwtToken(User user, bool isForRefreshToken = false)
    {
        ValidateIncomingUserData(user);

        string? secret, expiresAt;
        GetJwtConfigurationData(isForRefreshToken, out secret, out expiresAt);

        List<Claim> claims = GenerateJwtClaims(user);

        JwtSecurityToken token = GenerateJwtToken(secret, expiresAt, claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static JwtSecurityToken GenerateJwtToken(string? secret, string? expiresAt, List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(expiresAt)),
            signingCredentials: creds
        );
        return token;
    }

    private static List<Claim> GenerateJwtClaims(User user)
    {
        return new()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };
    }

    private static void GetJwtConfigurationData(bool isForRefreshToken, out string? secret, out string? expiresAt)
    {
        secret = AddConfigurationHelper.config.GetSection("Jwt:Secret").Value;
        if (!isForRefreshToken) expiresAt = AddConfigurationHelper.config.GetSection("Jwt:ExpirationInMinutes").Value;
        else expiresAt = AddConfigurationHelper.config.GetSection("Jwt:ExpirationInMinutesRefresh").Value;
    }

    private static void ValidateIncomingUserData(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
    }

    public static string GenerateRefreshToken(User user)
    {
        return GenerateJwtToken(user, true);
    }
}