using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportHelper.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class TwoVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "COL_OPERATIONALSYSTEM",
                table: "TB_MACHINE",
                type: "VARCHAR(45)",
                maxLength: 35,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 35);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "COL_OPERATIONALSYSTEM",
                table: "TB_MACHINE",
                type: "VARCHAR(30)",
                maxLength: 35,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(45)",
                oldMaxLength: 35,
                oldNullable: true);
        }
    }
}
