using Microsoft.EntityFrameworkCore;
using LoanManagement.Domain.Entities;

namespace LoanManagement.Infrastructure.Persistence;

public class LoanDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<EquipmentCategory> EquipmentCategories { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;
    public DbSet<LoanRequest> LoanRequests { get; set; } = null!;
    public DbSet<Loan> Loans { get; set; } = null!;

    public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USERS
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).HasColumnName("password").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Role).HasColumnName("role").IsRequired();
            // Enumは数値として保存
        });

        // EQUIPMENT_CATEGORIES
        modelBuilder.Entity<EquipmentCategory>(entity =>
        {
            entity.ToTable("EQUIPMENT_CATEGORIES");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description");
        });

        // EQUIPMENTS
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.ToTable("EQUIPMENTS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");

            entity.HasOne(e => e.Category)
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // LOAN_REQUESTS
        modelBuilder.Entity<LoanRequest>(entity =>
        {
            entity.ToTable("LOAN_REQUESTS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id").IsRequired();
            entity.Property(e => e.RequestDate).HasColumnName("request_date").IsRequired();
            entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
            entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            entity.Property(e => e.Purpose).HasColumnName("purpose").IsRequired();
            entity.Property(e => e.RejectionReason).HasColumnName("rejection_reason");

            entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Equipment>().WithMany().HasForeignKey(e => e.EquipmentId).OnDelete(DeleteBehavior.Restrict);
        });

        // LOANS
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("LOANS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LoanRequestId).HasColumnName("loan_request_id").IsRequired();
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id").IsRequired();
            entity.Property(e => e.LoanDate).HasColumnName("loan_date").IsRequired();
            entity.Property(e => e.DueDate).HasColumnName("due_date").IsRequired();
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();

            entity.HasOne<LoanRequest>().WithMany().HasForeignKey(e => e.LoanRequestId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Equipment>().WithMany().HasForeignKey(e => e.EquipmentId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
