using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class CompanySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanySetup",
                columns: table => new
                {
                    companyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    companyNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    chairmanId = table.Column<int>(type: "int", nullable: false),
                    companyLatitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    companyLongitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySetup", x => x.companyId);
                    table.ForeignKey(
                        name: "FK_CompanySetup_Employees_chairmanId",
                        column: x => x.chairmanId,
                        principalTable: "Employees",
                        principalColumn: "employeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySetup_chairmanId",
                table: "CompanySetup",
                column: "chairmanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySetup");
        }
    }
}
