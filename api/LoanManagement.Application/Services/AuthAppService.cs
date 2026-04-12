using LoanManagement.Application.DTOs.Auth;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Repositories;

namespace LoanManagement.Application.Services;

public class AuthAppService : IAuthAppService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthAppService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    #region ユーザーのログイン処理を行う

    /// <summary>
    /// ユーザーのログイン処理を行う
    /// </summary>
    /// <param name="request">ログイン情報</param>
    /// <returns></returns>
    public async Task<LoginResultDto?> LoginAsync(LoginRequestDto request)
    {
        // メールアドレスでユーザーを検索
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return null; // 該当ユーザーなし
        }

        // パスワードの検証
        if (user.PasswordHash != request.Password)
        {
            return null; // パスワード不一致
        }

        // インフラ層を用いてJWTを生成
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResultDto(token);
    }

    #endregion

    #region ユーザーのサインアップ処理を行う

    /// <summary>
    /// ユーザーのサインアップ処理を行う
    /// </summary>
    /// <param name="request">サインアップ情報</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<LoginResultDto?> SignupAsync(SignupRequestDto request)
    {
        // メールアドレス重複チェック
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("このメールアドレスは既に登録されています。");
        }

        var newUser = new Domain.Entities.User(
            id: 0,
            name: request.Name,
            email: request.Email,
            passwordHash: request.Password,
            role: Domain.Enums.Role.Employee
        );

        await _userRepository.AddAsync(newUser);

        // JWT生成
        var token = _jwtTokenGenerator.GenerateToken(newUser);
        return new LoginResultDto(token);
    }

    #endregion
}
