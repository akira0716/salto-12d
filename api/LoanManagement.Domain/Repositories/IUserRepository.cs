using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// ユーザーの取得
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <returns></returns>
    Task<User?> GetByIdAsync(int id);
    /// <summary>
    /// メールアドレスによるユーザーの取得（ログイン用）
    /// </summary>
    /// <param name="email">メールアドレス</param>
    /// <returns></returns>
    Task<User?> GetByEmailAsync(string email);
}
