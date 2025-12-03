using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompleteBanking_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTableAndFixUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Users",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Users");
        }
    }
}
