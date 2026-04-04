using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Tests.Entities;

public class LoanRequestTests
{
    [Fact]
    public void Constructor_EndDateBeforeStartDate_ThrowsException()
    {
        // Arrange
        var requestDate = new DateTime(2023, 1, 1);
        var startDate = new DateTime(2023, 1, 10);
        var endDate = new DateTime(2023, 1, 5); // Invalid: EndDate before StartDate

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            new LoanRequest(1, 1, 1, requestDate, startDate, endDate, "Test Purpose"));
        Assert.Equal("利用終了希望日は利用開始希望日より後の日付でなければなりません。", exception.Message);
    }

    [Fact]
    public void Approve_WhenNotPending_ThrowsException()
    {
        // Arrange
        var request = CreateValidPendingRequest();
        request.Approve(); // Status becomes Approved

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => request.Approve());
        Assert.Equal("申請中以外のステータスから承認することはできません。", exception.Message);
    }

    [Fact]
    public void Reject_WhenNotPending_ThrowsException()
    {
        // Arrange
        var request = CreateValidPendingRequest();
        request.Approve(); // Status becomes Approved

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => request.Reject("Reason"));
        Assert.Equal("申請中以外のステータスから却下することはできません。", exception.Message);
    }

    [Fact]
    public void Reject_WithEmptyReason_ThrowsException()
    {
        // Arrange
        var request = CreateValidPendingRequest();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => request.Reject(string.Empty));
        Assert.Equal("却下する場合は却下理由が必要です。", exception.Message);
    }

    [Fact]
    public void Approve_Valid_UpdatesStatus()
    {
        // Arrange
        var request = CreateValidPendingRequest();

        // Act
        request.Approve();

        // Assert
        Assert.Equal(LoanRequestStatus.Approved, request.Status);
    }

    [Fact]
    public void Reject_Valid_UpdatesStatusAndReason()
    {
        // Arrange
        var request = CreateValidPendingRequest();
        var rejectReason = "No longer available";

        // Act
        request.Reject(rejectReason);

        // Assert
        Assert.Equal(LoanRequestStatus.Rejected, request.Status);
        Assert.Equal(rejectReason, request.RejectionReason);
    }

    private static LoanRequest CreateValidPendingRequest()
    {
        return new LoanRequest(
            id: 1,
            userId: 1,
            equipmentId: 1,
            requestDate: new DateTime(2023, 1, 1),
            startDate: new DateTime(2023, 1, 5),
            endDate: new DateTime(2023, 1, 10),
            purpose: "Test");
    }
}
