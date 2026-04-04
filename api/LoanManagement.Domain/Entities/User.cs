using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.Entities;

/// <summary>
/// ユーザーエンティティ
/// </summary>
public class User
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// ユーザー名
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// メールアドレス
    /// </summary>
    public string Email { get; private set; }
    /// <summary>
    /// パスワードハッシュ
    /// </summary>
    public string PasswordHash { get; private set; }
    /// <summary>
    /// 権限
    /// </summary>
    public Role Role { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <param name="name">ユーザー名</param>
    /// <param name="email">メールアドレス</param>
    /// <param name="passwordHash">パスワードハッシュ</param>
    /// <param name="role">権限</param>
    public User(int id, string name, string email, string passwordHash, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    /// <summary>
    /// 管理者判定処理
    /// </summary>
    /// <returns></returns>
    public bool IsAdmin()
    {
        return Role == Role.Admin;
    }
}
