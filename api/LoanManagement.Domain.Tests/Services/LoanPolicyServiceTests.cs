using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;
using LoanManagement.Domain.Services;

namespace LoanManagement.Domain.Tests.Services;

public class LoanPolicyServiceTests
{
    private readonly LoanPolicyService _service;
    private readonly User _testUser;

    public LoanPolicyServiceTests()
    {
        _service = new LoanPolicyService();
        _testUser = new User(1, "Test User", "test@example.com", "hash", Role.Employee);
    }

    [Fact]
    public void ValidateLoanRequest_DurationOver30Days_ThrowsException()
    {
        // Arrange
        var currentDate = new DateTime(2023, 1, 1);
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 2, 1); // 31 days

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _service.ValidateLoanRequest(_testUser, new List<LoanRequest>(), new List<Loan>(), currentDate, startDate, endDate));
        Assert.Equal("貸出期間は1日以上、最大30日までです。", exception.Message);
    }

    [Fact]
    public void ValidateLoanRequest_DurationZeroOrNegative_ThrowsException()
    {
        // Arrange
        var currentDate = new DateTime(2023, 1, 1);
        var startDate = new DateTime(2023, 1, 2);
        var endDate = new DateTime(2023, 1, 1); // Negative duration

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _service.ValidateLoanRequest(_testUser, new List<LoanRequest>(), new List<Loan>(), currentDate, startDate, endDate));
        Assert.Equal("貸出期間は1日以上、最大30日までです。", exception.Message);
    }

    [Fact]
    public void ValidateLoanRequest_HasOverdueLoans_ThrowsException()
    {
        // Arrange
        var currentDate = new DateTime(2023, 1, 21);
        var startDate = new DateTime(2023, 1, 22);
        var endDate = new DateTime(2023, 1, 25);

        var activeLoans = new List<Loan>
        {
            new Loan(1, 1, 1, 1, new DateTime(2023, 1, 10), new DateTime(2023, 1, 20)) // Overdue loan
        };

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _service.ValidateLoanRequest(_testUser, new List<LoanRequest>(), activeLoans, currentDate, startDate, endDate));
        Assert.Equal("遅延している貸出があるため、新規の申請を行うことはできません。", exception.Message);
    }

    [Fact]
    public void ValidateLoanRequest_MoreThan3ActiveItems_ThrowsException()
    {
        // Arrange
        var currentDate = new DateTime(2023, 1, 1);
        var startDate = new DateTime(2023, 1, 2);
        var endDate = new DateTime(2023, 1, 5);

        var pendingRequests = new List<LoanRequest>
        {
            new LoanRequest(1, 1, 1, currentDate, startDate, endDate, "Purpose 1"),
            new LoanRequest(2, 1, 2, currentDate, startDate, endDate, "Purpose 2")
        };

        var activeLoans = new List<Loan>
        {
            new Loan(1, 3, 1, 3, currentDate, new DateTime(2023, 1, 10)) // Active loan
        };
        // Total active items = 3, which is the limit limit for current state, adding a new request will mean it fails validation.
        // Wait, the policy says: "If current total >= 3, reject". So passing 2 requests + 1 loan = 3 current items.

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            _service.ValidateLoanRequest(_testUser, pendingRequests, activeLoans, currentDate, startDate, endDate));
        Assert.Equal("「申請中」および「貸出中」の合計が最大3件に達しているため、これ以上の申請はできません。", exception.Message);
    }

    [Fact]
    public void ValidateLoanRequest_ValidRequest_DoesNotThrow()
    {
        // Arrange
        var currentDate = new DateTime(2023, 1, 1);
        var startDate = new DateTime(2023, 1, 2);
        var endDate = new DateTime(2023, 1, 15); // 13 days

        var pendingRequests = new List<LoanRequest>
        {
            new LoanRequest(1, 1, 1, currentDate, startDate, endDate, "Purpose 1")
        };

        var activeLoans = new List<Loan>
        {
            new Loan(1, 2, 1, 2, currentDate, new DateTime(2023, 1, 20)) // Active, not overdue
        };
        // Total active items = 2

        // Act
        // Shouldn't throw
        _service.ValidateLoanRequest(_testUser, pendingRequests, activeLoans, currentDate, startDate, endDate);

        // Assert: No exception is thrown
        Assert.True(true);
    }
}
