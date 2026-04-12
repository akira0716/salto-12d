using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanManagement.Infrastructure.Authentication;

/// <summary>
/// JWTトークンの生成を担当するクラス
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;

    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// ユーザー情報を元にJWTトークンを生成する
    /// </summary>
    /// <param name="user">ユーザー情報</param>
    /// <returns></returns>
    public string GenerateToken(User user)
    {
        // クレームの作成
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            // 管理者かどうかのロール判定
            new Claim(ClaimTypes.Role, user.IsAdmin() ? "Admin" : "Employee")
        };
        // キーの生成
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        // 暗号化
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        // JWTの生成
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
