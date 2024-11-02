using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wedev.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Tenants_TenantId",
                table: "App");

            migrationBuilder.DropTable(
                name: "Mandanten");

            migrationBuilder.DropPrimaryKey(
                name: "PK_App",
                table: "App");

            migrationBuilder.RenameTable(
                name: "App",
                newName: "Apps");

            migrationBuilder.RenameIndex(
                name: "IX_App_TenantId",
                table: "Apps",
                newName: "IX_Apps_TenantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Apps",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Apps",
                table: "Apps",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apps_Tenants_TenantId",
                table: "Apps",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apps_Tenants_TenantId",
                table: "Apps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Apps",
                table: "Apps");

            migrationBuilder.RenameTable(
                name: "Apps",
                newName: "App");

            migrationBuilder.RenameIndex(
                name: "IX_Apps_TenantId",
                table: "App",
                newName: "IX_App_TenantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "App",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_App",
                table: "App",
                column: "AppId");

            migrationBuilder.CreateTable(
                name: "Mandanten",
                columns: table => new
                {
                    MandantId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mandanten", x => x.MandantId);
                    table.ForeignKey(
                        name: "FK_Mandanten_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mandanten_TenantId",
                table: "Mandanten",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Tenants_TenantId",
                table: "App",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }
    }
}
