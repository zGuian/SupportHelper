using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportHelper.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_MACHINE",
                columns: table => new
                {
                    COL_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COL_HOSTNAME = table.Column<string>(type: "CHAR(14)", maxLength: 15, nullable: false),
                    COL_ISCONNECTED = table.Column<bool>(type: "BIT", nullable: false),
                    COL_SGPISRUNNING = table.Column<bool>(type: "BIT", nullable: false),
                    COL_CURRENTUSERNAME = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    COL_DOMAINNAME = table.Column<string>(type: "VARCHAR(25)", maxLength: 25, nullable: false),
                    COL_UPTIME = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    COL_OPERATIONALSYSTEM = table.Column<string>(type: "VARCHAR(30)", maxLength: 35, nullable: false),
                    COL_SIGNALR_CONNECTIONID = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    COL_SIGNALR_ISACTIVE = table.Column<bool>(type: "BIT", nullable: false),
                    COL_LASTUPDATE = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MACHINE", x => x.COL_ID);
                    table.UniqueConstraint("AK_TB_MACHINE_COL_HOSTNAME", x => x.COL_HOSTNAME);
                });

            migrationBuilder.CreateTable(
                name: "TB_NETWORKBOARD",
                columns: table => new
                {
                    COL_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_MACHINE = table.Column<int>(type: "int", nullable: false),
                    COL_IPV4 = table.Column<string>(type: "CHAR(15)", nullable: false),
                    COL_MACADRESS = table.Column<string>(type: "CHAR(12)", nullable: false),
                    COL_INUSE = table.Column<bool>(type: "BIT", nullable: false),
                    COL_DESCRIPTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    COL_IPV6 = table.Column<string>(type: "VARCHAR(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NETWORKBOARD", x => x.COL_ID);
                    table.UniqueConstraint("AK_TB_NETWORKBOARD_COL_MACADRESS", x => x.COL_MACADRESS);
                    table.ForeignKey(
                        name: "FK_TB_NETWORKBOARD_TB_MACHINE_FK_MACHINE",
                        column: x => x.FK_MACHINE,
                        principalTable: "TB_MACHINE",
                        principalColumn: "COL_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_MACHINE_COL_HOSTNAME",
                table: "TB_MACHINE",
                column: "COL_HOSTNAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_NETWORKBOARD_FK_MACHINE",
                table: "TB_NETWORKBOARD",
                column: "FK_MACHINE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_NETWORKBOARD");

            migrationBuilder.DropTable(
                name: "TB_MACHINE");
        }
    }
}
