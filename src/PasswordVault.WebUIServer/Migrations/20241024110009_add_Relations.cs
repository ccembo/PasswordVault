using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pv.Migrations
{
    /// <inheritdoc />
    public partial class add_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_UserVault_User_UserId",
                table: "UserVault",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVault_Vault_VaultId",
                table: "UserVault",
                column: "VaultId",
                principalTable: "Vault",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserVault_User_UserId",
                table: "UserVault");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVault_Vault_VaultId",
                table: "UserVault");
        }
    }
}
