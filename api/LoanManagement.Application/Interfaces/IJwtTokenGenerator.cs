using LoanManagement.Domain.Entities;

namespace LoanManagement.Application.Interfaces;

/// <summary>
/// インフラと連携してJWTを生成するためのインターフェース
/// </summary>
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
