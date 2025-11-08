using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarChanger.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Hidden = table.Column<bool>(type: "INTEGER", nullable: false),
                    CustomStart = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    CustomEnd = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    CustomTitle = table.Column<string>(type: "TEXT", nullable: true),
                    OriginalTitle = table.Column<string>(type: "TEXT", nullable: true),
                    OriginalStart = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    OriginalEnd = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    LastHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
