using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Tests.Entities;

public class LoanTests
{
    [Fact]
    public void Constructor_DueDateBeforeLoanDate_ThrowsException()
    {
        // Arrange
        var loanDate = new DateTime(2023, 1, 10);
        var dueDate = new DateTime(2023, 1, 5); // Invalid: DueDate before LoanDate

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            new Loan(1, 1, 1, 1, loanDate, dueDate));
        Assert.Equal("返却予定日は貸出日より後の日付でなければなりません。", exception.Message);
    }

    [Fact]
    public void Return_WhenNotActive_ThrowsException()
    {
        // Arrange
        var loan = CreateValidActiveLoan();
        loan.Return(new DateTime(2023, 1, 15)); // Status becomes Returned

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => loan.Return(new DateTime(2023, 1, 16)));
        Assert.Equal("貸出中ではないため返却できません。", exception.Message);
    }

    [Fact]
    public void Return_Valid_UpdatesStatusAndReturnDate()
    {
        // Arrange
        var loan = CreateValidActiveLoan();
        var returnDate = new DateTime(2023, 1, 15);

        // Act
        loan.Return(returnDate);

        // Assert
        Assert.Equal(LoanStatus.Returned, loan.Status);
        Assert.Equal(returnDate, loan.ReturnDate);
    }

    [Fact]
    public void IsOverdue_WhenActiveAndPastDueDate_ReturnsTrue()
    {
        // Arrange
        var loan = CreateValidActiveLoan(); // DueDate is 2023-01-20
        var currentDate = new DateTime(2023, 1, 21);

        // Act
        var isOverdue = loan.IsOverdue(currentDate);

        // Assert
        Assert.True(isOverdue);
    }

    [Fact]
    public void IsOverdue_WhenActiveAndBeforeDueDate_ReturnsFalse()
    {
        // Arrange
        var loan = CreateValidActiveLoan(); // DueDate is 2023-01-20
        var currentDate = new DateTime(2023, 1, 19);

        // Act
        var isOverdue = loan.IsOverdue(currentDate);

        // Assert
        Assert.False(isOverdue);
    }

    [Fact]
    public void IsOverdue_WhenReturned_ReturnsFalse()
    {
        // Arrange
        var loan = CreateValidActiveLoan(); // DueDate is 2023-01-20
        loan.Return(new DateTime(2023, 1, 15));
        var currentDate = new DateTime(2023, 1, 21); // Past due date but already returned

        // Act
        var isOverdue = loan.IsOverdue(currentDate);

        // Assert
        Assert.False(isOverdue);
    }

    private static Loan CreateValidActiveLoan()
    {
        return new Loan(
            id: 1,
            loanRequestId: 1,
            userId: 1,
            equipmentId: 1,
            loanDate: new DateTime(2023, 1, 10),
            dueDate: new DateTime(2023, 1, 20));
    }
}
