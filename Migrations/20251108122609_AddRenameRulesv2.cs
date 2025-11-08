using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarChanger.Migrations
{
    /// <inheritdoc />
    public partial class AddRenameRulesv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "RenameRules",
                newName: "Replace");

            migrationBuilder.AddColumn<string>(
                name: "Match",
                table: "RenameRules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Match",
                table: "RenameRules");

            migrationBuilder.RenameColumn(
                name: "Replace",
                table: "RenameRules",
                newName: "Url");
        }
    }
}
