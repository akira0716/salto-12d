using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Repositories;

namespace LoanManagement.Application.Services;

/// <summary>
/// 貸出状況管理および返却に関するアプリケーションサービスの実装
/// </summary>
public class LoanAppService : ILoanAppService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public LoanAppService(ILoanRepository loanRepository, IUserRepository userRepository, IEquipmentRepository equipmentRepository)
    {
        _loanRepository = loanRepository;
        _userRepository = userRepository;
        _equipmentRepository = equipmentRepository;
    }

    #region 全ての貸出情報一覧を取得（管理者向け）

    /// <summary>
    /// 全ての貸出情報一覧を取得（管理者向け）
    /// </summary>
    /// <returns>貸出状況情報の一覧</returns>
    public async Task<IEnumerable<LoanDto>> GetAllAsync()
    {
        // 全ての貸出情報を取得
        var allLoans = await _loanRepository.GetAllAsync();
        return await MapToDtoAsync(allLoans);
    }

    #endregion

    #region 指定されたユーザーの現在有効な「貸出中」情報一覧を取得

    /// <summary>
    /// 指定されたユーザーの現在有効な「貸出中」情報一覧を取得
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns>当該ユーザーの現在貸出中の情報一覧</returns>
    public async Task<IEnumerable<LoanDto>> GetActiveByUserIdAsync(int userId)
    {
        // 指定されたユーザーの現在有効な「貸出中」情報を取得
        var activeLoans = await _loanRepository.GetActiveLoansByUserIdAsync(userId);
        return await MapToDtoAsync(activeLoans);
    }

    #endregion

    #region 備品の返却処理（管理者向け）

    /// <summary>
    /// 備品の返却処理（管理者向け）
    /// </summary>
    /// <param name="loanId">貸出ID</param>
    public async Task ReturnEquipmentAsync(int loanId)
    {
        // 貸出情報を取得
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null) throw new DomainException("貸出情報が見つかりません。");

        // 返却処理
        loan.Return(DateTime.Now.Date);
        await _loanRepository.UpdateAsync(loan);

        // 連携して備品ステータスを利用可へ戻す
        var equipment = await _equipmentRepository.GetByIdAsync(loan.EquipmentId);
        if (equipment != null)
        {
            try
            {
                // 備品ステータスを利用可能へ変更
                equipment.ChangeStatus(Domain.Enums.EquipmentStatus.Available);
                await _equipmentRepository.UpdateAsync(equipment);
            }
            catch (DomainException)
            {
                // 例えば修理中（故障により）の場合はそのままにする等の処理。
                // 今回はシンプルにしています。
            }
        }
    }

    #endregion

    /// <summary>
    /// LoanエンティティのリストをLoanDtoのリストに変換するヘルパーメソッド
    /// </summary>
    /// <param name="loans"></param>
    /// <returns></returns>
    private async Task<IEnumerable<LoanDto>> MapToDtoAsync(IEnumerable<Loan> loans)
    {
        var result = new List<LoanDto>();
        var currentDate = DateTime.Now.Date;

        foreach (var loan in loans)
        {
            var user = await _userRepository.GetByIdAsync(loan.UserId);
            var eq = await _equipmentRepository.GetByIdAsync(loan.EquipmentId);

            result.Add(new LoanDto
            {
                Id = loan.Id,
                LoanRequestId = loan.LoanRequestId,
                UserId = loan.UserId,
                UserName = user?.Name ?? "不明",
                EquipmentId = loan.EquipmentId,
                EquipmentName = eq?.Name ?? "不明",
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = loan.Status,
                IsOverdue = loan.IsOverdue(currentDate)
            });
        }
        return result;
    }
}
