using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_user_fullname_tg_username_idx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_users_fullname_telegram_username",
                table: "users",
                columns: new[] { "fullname", "telegram_username" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_fullname_telegram_username",
                table: "users");
        }
    }
}
