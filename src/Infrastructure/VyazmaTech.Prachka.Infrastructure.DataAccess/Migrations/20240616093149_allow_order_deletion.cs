using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class allow_order_deletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_queues_queue_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_users_user_id",
                table: "orders");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_queues_queue_id",
                table: "orders",
                column: "queue_id",
                principalTable: "queues",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_users_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_queues_queue_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_users_user_id",
                table: "orders");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_queues_queue_id",
                table: "orders",
                column: "queue_id",
                principalTable: "queues",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_users_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
