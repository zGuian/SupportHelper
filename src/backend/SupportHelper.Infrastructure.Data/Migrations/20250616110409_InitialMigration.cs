using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportHelper.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "machine",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    hostname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    current_username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    domain_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    operational_system = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machine", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "machine");
        }
    }
}
