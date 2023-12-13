using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SplitSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Subscriptions_SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionEntityId",
                table: "Orders",
                newName: "OrderSubscriptionEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SubscriptionEntityId",
                table: "Orders",
                newName: "IX_Orders_OrderSubscriptionEntityId");

            migrationBuilder.AddColumn<Guid>(
                name: "QueueSubscriptionEntityId",
                table: "Queues",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueueSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Queues_QueueSubscriptionEntityId",
                table: "Queues",
                column: "QueueSubscriptionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSubscriptions_UserId",
                table: "OrderSubscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueueSubscriptions_UserId",
                table: "QueueSubscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderSubscriptions_OrderSubscriptionEntityId",
                table: "Orders",
                column: "OrderSubscriptionEntityId",
                principalTable: "OrderSubscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_QueueSubscriptions_QueueSubscriptionEntityId",
                table: "Queues",
                column: "QueueSubscriptionEntityId",
                principalTable: "QueueSubscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderSubscriptions_OrderSubscriptionEntityId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Queues_QueueSubscriptions_QueueSubscriptionEntityId",
                table: "Queues");

            migrationBuilder.DropTable(
                name: "OrderSubscriptions");

            migrationBuilder.DropTable(
                name: "QueueSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Queues_QueueSubscriptionEntityId",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "QueueSubscriptionEntityId",
                table: "Queues");

            migrationBuilder.RenameColumn(
                name: "OrderSubscriptionEntityId",
                table: "Orders",
                newName: "SubscriptionEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OrderSubscriptionEntityId",
                table: "Orders",
                newName: "IX_Orders_SubscriptionEntityId");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QueueId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_QueueId",
                table: "Subscriptions",
                column: "QueueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Subscriptions_SubscriptionEntityId",
                table: "Orders",
                column: "SubscriptionEntityId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
