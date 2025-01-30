using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class Last_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRequests_Accounts_accountId",
                table: "AttendanceRequests");

            migrationBuilder.RenameColumn(
                name: "accountId",
                table: "AttendanceRequests",
                newName: "employeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AttendanceRequests_accountId",
                table: "AttendanceRequests",
                newName: "IX_AttendanceRequests_employeeId");

            migrationBuilder.AlterColumn<int>(
                name: "machineCode",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "AttendanceRequests",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRequests_Employees_employeeId",
                table: "AttendanceRequests",
                column: "employeeId",
                principalTable: "Employees",
                principalColumn: "employeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRequests_Employees_employeeId",
                table: "AttendanceRequests");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "AttendanceRequests",
                newName: "accountId");

            migrationBuilder.RenameIndex(
                name: "IX_AttendanceRequests_employeeId",
                table: "AttendanceRequests",
                newName: "IX_AttendanceRequests_accountId");

            migrationBuilder.AlterColumn<int>(
                name: "machineCode",
                table: "Attendances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "AttendanceRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRequests_Accounts_accountId",
                table: "AttendanceRequests",
                column: "accountId",
                principalTable: "Accounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
