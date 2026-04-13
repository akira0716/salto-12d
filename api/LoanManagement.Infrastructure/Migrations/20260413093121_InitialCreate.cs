using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EQUIPMENT_CATEGORIES",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENT_CATEGORIES", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EQUIPMENTS",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    category_id = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EQUIPMENTS", x => x.id);
                    table.ForeignKey(
                        name: "FK_EQUIPMENTS_EQUIPMENT_CATEGORIES_category_id",
                        column: x => x.category_id,
                        principalTable: "EQUIPMENT_CATEGORIES",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_REQUESTS",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    equipment_id = table.Column<int>(type: "INTEGER", nullable: false),
                    request_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    start_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    end_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    purpose = table.Column<string>(type: "TEXT", nullable: false),
                    rejection_reason = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_REQUESTS", x => x.id);
                    table.ForeignKey(
                        name: "FK_LOAN_REQUESTS_EQUIPMENTS_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LOAN_REQUESTS_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LOANS",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    loan_request_id = table.Column<int>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    equipment_id = table.Column<int>(type: "INTEGER", nullable: false),
                    loan_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    due_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    return_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOANS", x => x.id);
                    table.ForeignKey(
                        name: "FK_LOANS_EQUIPMENTS_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LOANS_LOAN_REQUESTS_loan_request_id",
                        column: x => x.loan_request_id,
                        principalTable: "LOAN_REQUESTS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LOANS_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENTS_category_id",
                table: "EQUIPMENTS",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_REQUESTS_equipment_id",
                table: "LOAN_REQUESTS",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_REQUESTS_user_id",
                table: "LOAN_REQUESTS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_LOANS_equipment_id",
                table: "LOANS",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_LOANS_loan_request_id",
                table: "LOANS",
                column: "loan_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_LOANS_user_id",
                table: "LOANS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_email",
                table: "USERS",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOANS");

            migrationBuilder.DropTable(
                name: "LOAN_REQUESTS");

            migrationBuilder.DropTable(
                name: "EQUIPMENTS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "EQUIPMENT_CATEGORIES");
        }
    }
}
