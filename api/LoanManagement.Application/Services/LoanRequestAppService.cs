using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Repositories;
using LoanManagement.Domain.Services;

namespace LoanManagement.Application.Services;

public class LoanRequestAppService : ILoanRequestAppService
{
    private readonly ILoanRequestRepository _requestRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly LoanPolicyService _policyService;

    public LoanRequestAppService(
        ILoanRequestRepository requestRepository,
        ILoanRepository loanRepository,
        IUserRepository userRepository,
        IEquipmentRepository equipmentRepository,
        LoanPolicyService policyService)
    {
        _requestRepository = requestRepository;
        _loanRepository = loanRepository;
        _userRepository = userRepository;
        _equipmentRepository = equipmentRepository;
        _policyService = policyService;
    }

    #region 全ての貸出申請の一覧を取得（管理者向け）

    /// <summary>
    /// 全ての貸出申請の一覧を取得（管理者向け）
    /// </summary>
    /// <returns>貸出申請情報の一覧</returns>
    public async Task<IEnumerable<LoanRequestDto>> GetAllAsync()
    {
        // 貸出申請の全件を取得
        var requests = await _requestRepository.GetAllAsync();
        return await MapToDtoAsync(requests);
    }

    #endregion

    #region 指定されたユーザーの貸出申請一覧を取得

    /// <summary>
    /// 指定されたユーザーの貸出申請一覧を取得
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns>当該ユーザーの貸出申請情報の一覧</returns>
    public async Task<IEnumerable<LoanRequestDto>> GetMyRequestsAsync(int userId)
    {
        // リポジトリ側でユーザーIDによる絞り込みを実施
        var mine = await _requestRepository.GetAllAsync(userId: userId);
        return await MapToDtoAsync(mine);
    }

    #endregion

    #region 貸出ポリシーを検証した上で、新しい貸出申請を登録

    /// <summary>
    /// 貸出ポリシーを検証した上で、新しい貸出申請を登録
    /// </summary>
    /// <param name="userId">申請するユーザーID</param>
    /// <param name="dto">申請内容</param>
    /// <returns>登録された申請のID</returns>
    public async Task<int> CreateAsync(int userId, LoanRequestCreateDto dto)
    {
        // ユーザーの存在確認
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new DomainException("ユーザーが見つかりません。");

        // 貸出ポリシー検証のため、ユーザーの現在の申請と貸出を取得
        var pendingRequests = await _requestRepository.GetPendingRequestsByUserIdAsync(user.Id);
        var activeLoans = await _loanRepository.GetActiveLoansByUserIdAsync(user.Id);

        // ドメインサービスを使用してポリシー検証
        _policyService.ValidateLoanRequest(user, pendingRequests, activeLoans, DateTime.Now.Date, dto.StartDate, dto.EndDate);

        // 貸出申請を作成（IDはリポジトリで生成）
        var request = new LoanRequest(0, user.Id, dto.EquipmentId, DateTime.Now.Date, dto.StartDate, dto.EndDate, dto.Purpose);
        await _requestRepository.AddAsync(request);

        return request.Id;
    }

    #endregion

    #region 貸出申請を承認し、貸出処理を実行（管理者向け）

    /// <summary>
    /// 貸出申請を承認し、貸出処理を実行（管理者向け）
    /// </summary>
    /// <param name="requestId">申請ID</param>
    public async Task ApproveAsync(int requestId)
    {
        // 申請の存在確認
        var request = await _requestRepository.GetByIdAsync(requestId);
        if (request == null) throw new DomainException("申請が見つかりません。");

        // 承認状態にする
        request.Approve();

        // 貸出を作成する（IDはリポジトリで生成）
        var loan = new Loan(0, request.Id, request.UserId, request.EquipmentId, DateTime.Now.Date, request.EndDate);

        // トランザクションを考慮して、申請の状態更新と貸出の追加を同時に行う
        await _requestRepository.UpdateAsync(request);
        await _loanRepository.AddAsync(loan);

        // 対象備品を貸出中にする
        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
        if (equipment != null)
        {
            equipment.ChangeStatus(Domain.Enums.EquipmentStatus.Loaned);
            await _equipmentRepository.UpdateAsync(equipment);
        }
    }

    #endregion

    #region 貸出申請を却下（管理者向け）

    /// <summary>
    /// 貸出申請を却下（管理者向け）
    /// </summary>
    /// <param name="requestId">申請ID</param>
    /// <param name="dto">却下理由等の情報</param>
    public async Task RejectAsync(int requestId, LoanRequestRejectDto dto)
    {
        // 申請の存在確認
        var request = await _requestRepository.GetByIdAsync(requestId);
        if (request == null) throw new DomainException("申請が見つかりません。");

        // 却下状態にする
        request.Reject(dto.RejectionReason);
        await _requestRepository.UpdateAsync(request);

        // もし備品の故障も同時に報告された場合は、備品の状態を修理中にする
        if (dto.SetEquipmentBroken)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId);
            if (equipment != null)
            {
                equipment.ChangeStatus(Domain.Enums.EquipmentStatus.UnderRepair);
                await _equipmentRepository.UpdateAsync(equipment);
            }
        }
    }

    #endregion

    /// <summary>
    /// LoanRequestエンティティのリストをLoanRequestDtoのリストに変換する
    /// </summary>
    /// <param name="requests"></param>
    /// <returns></returns>
    private async Task<IEnumerable<LoanRequestDto>> MapToDtoAsync(IEnumerable<LoanRequest> requests)
    {
        var result = new List<LoanRequestDto>();
        foreach (var req in requests)
        {
            var user = await _userRepository.GetByIdAsync(req.UserId);
            var eq = await _equipmentRepository.GetByIdAsync(req.EquipmentId);

            result.Add(new LoanRequestDto
            {
                Id = req.Id,
                UserId = req.UserId,
                UserName = user?.Name ?? "不明",
                EquipmentId = req.EquipmentId,
                EquipmentName = eq?.Name ?? "不明",
                RequestDate = req.RequestDate,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Status = req.Status,
                Purpose = req.Purpose,
                RejectionReason = req.RejectionReason
            });
        }
        return result;
    }
}
