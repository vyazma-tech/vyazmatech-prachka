using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeparateSubscriptions_DateOnlyTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Subscriptions_SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreationDate",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreationDate",
                table: "Queues",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreationDate",
                table: "Orders",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "OrderSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateOnly>(type: "date", nullable: false),
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
                    CreationDate = table.Column<DateOnly>(type: "date", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "OrderEntityOrderSubscriptionEntity",
                columns: table => new
                {
                    OrderSubscriptionEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribedOrdersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEntityOrderSubscriptionEntity", x => new { x.OrderSubscriptionEntityId, x.SubscribedOrdersId });
                    table.ForeignKey(
                        name: "FK_OrderEntityOrderSubscriptionEntity_OrderSubscriptions_Order~",
                        column: x => x.OrderSubscriptionEntityId,
                        principalTable: "OrderSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderEntityOrderSubscriptionEntity_Orders_SubscribedOrdersId",
                        column: x => x.SubscribedOrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueueEntityQueueSubscriptionEntity",
                columns: table => new
                {
                    QueueSubscriptionEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribedQueuesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueEntityQueueSubscriptionEntity", x => new { x.QueueSubscriptionEntityId, x.SubscribedQueuesId });
                    table.ForeignKey(
                        name: "FK_QueueEntityQueueSubscriptionEntity_QueueSubscriptions_Queue~",
                        column: x => x.QueueSubscriptionEntityId,
                        principalTable: "QueueSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueueEntityQueueSubscriptionEntity_Queues_SubscribedQueuesId",
                        column: x => x.SubscribedQueuesId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderEntityOrderSubscriptionEntity_SubscribedOrdersId",
                table: "OrderEntityOrderSubscriptionEntity",
                column: "SubscribedOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSubscriptions_UserId",
                table: "OrderSubscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueueEntityQueueSubscriptionEntity_SubscribedQueuesId",
                table: "QueueEntityQueueSubscriptionEntity",
                column: "SubscribedQueuesId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueSubscriptions_UserId",
                table: "QueueSubscriptions",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderEntityOrderSubscriptionEntity");

            migrationBuilder.DropTable(
                name: "QueueEntityQueueSubscriptionEntity");

            migrationBuilder.DropTable(
                name: "OrderSubscriptions");

            migrationBuilder.DropTable(
                name: "QueueSubscriptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Queues",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionEntityId",
                table: "Orders",
                type: "uuid",
                nullable: true);

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
                name: "IX_Orders_SubscriptionEntityId",
                table: "Orders",
                column: "SubscriptionEntityId");

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
