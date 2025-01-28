using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departmentNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    departmentNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Arabic_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    machineCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    machineName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    machineIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    machinePort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.machineCode);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    roleNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fullNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Arabic_CI_AS"),
                    departmentId = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    userPhotoPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    isCheckedIn = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.employeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    accountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.accountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "employeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    attendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    machineCode = table.Column<int>(type: "int", nullable: false),
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    verifyMode = table.Column<int>(type: "int", nullable: false),
                    checkType = table.Column<int>(type: "int", nullable: false),
                    checkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.attendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "employeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Machines_machineCode",
                        column: x => x.machineCode,
                        principalTable: "Machines",
                        principalColumn: "machineCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Ix_Accounts_employeeId",
                table: "Accounts",
                column: "employeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Ix_Accounts_roleId",
                table: "Accounts",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "Ix_Attendances_employeeId",
                table: "Attendances",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "Ix_Attendances_machineCode",
                table: "Attendances",
                column: "machineCode");

            migrationBuilder.CreateIndex(
                name: "Ix_Employees_departmentId",
                table: "Employees",
                column: "departmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
