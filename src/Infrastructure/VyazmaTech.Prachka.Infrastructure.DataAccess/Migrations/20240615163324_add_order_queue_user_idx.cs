using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_order_queue_user_idx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_orders_queue_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_user_id",
                table: "orders");

            migrationBuilder.CreateIndex(
                name: "ix_orders_queue_id_user_id",
                table: "orders",
                columns: new[] { "queue_id", "user_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_orders_queue_id_user_id",
                table: "orders");

            migrationBuilder.CreateIndex(
                name: "ix_orders_queue_id",
                table: "orders",
                column: "queue_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_user_id",
                table: "orders",
                column: "user_id");
        }
    }
}
