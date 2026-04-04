using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Tests.Entities;

public class EquipmentTests
{
    [Fact]
    public void ChangeStatus_FromDisposed_ThrowsException()
    {
        // Arrange
        var equipment = new Equipment(1, "Test Equipment", 1, EquipmentStatus.Disposed, "Test Description");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => equipment.ChangeStatus(EquipmentStatus.Available));
        Assert.Equal("廃棄済の備品の状態は変更できません。", exception.Message);
    }

    [Fact]
    public void ChangeStatus_FromUnderRepairToLoaned_ThrowsException()
    {
        // Arrange
        var equipment = new Equipment(1, "Test Equipment", 1, EquipmentStatus.UnderRepair, "Test Description");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => equipment.ChangeStatus(EquipmentStatus.Loaned));
        Assert.Equal("修理中の備品を直接貸出中に変更することはできません。", exception.Message);
    }

    [Fact]
    public void ChangeStatus_ValidTransition_UpdatesStatus()
    {
        // Arrange
        var equipment = new Equipment(1, "Test Equipment", 1, EquipmentStatus.Available, "Test Description");

        // Act
        equipment.ChangeStatus(EquipmentStatus.Loaned);

        // Assert
        Assert.Equal(EquipmentStatus.Loaned, equipment.Status);
    }
}
