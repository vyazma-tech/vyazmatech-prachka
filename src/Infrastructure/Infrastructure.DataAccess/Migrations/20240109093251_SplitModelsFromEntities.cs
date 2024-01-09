using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SplitModelsFromEntities : Migration
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
                name: "IX_Queues_CreationDate",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreationDate",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "Expired",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderSubscriptionId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QueueSubscriptionId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateOnly>(
                name: "RegistrationDate",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "AssignmentDate",
                table: "Queues",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

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
                name: "UserOrdersAndTheirSubscriptions",
                columns: table => new
                {
                    OrderSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrdersAndTheirSubscriptions", x => new { x.OrderSubscriptionId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_UserOrdersAndTheirSubscriptions_OrderSubscriptions_OrderSub~",
                        column: x => x.OrderSubscriptionId,
                        principalTable: "OrderSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrdersAndTheirSubscriptions_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQueuesAndTheirSubscriptions",
                columns: table => new
                {
                    QueueSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    QueuesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQueuesAndTheirSubscriptions", x => new { x.QueueSubscriptionId, x.QueuesId });
                    table.ForeignKey(
                        name: "FK_UserQueuesAndTheirSubscriptions_QueueSubscriptions_QueueSub~",
                        column: x => x.QueueSubscriptionId,
                        principalTable: "QueueSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQueuesAndTheirSubscriptions_Queues_QueuesId",
                        column: x => x.QueuesId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_UserOrdersAndTheirSubscriptions_OrdersId",
                table: "UserOrdersAndTheirSubscriptions",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQueuesAndTheirSubscriptions_QueuesId",
                table: "UserQueuesAndTheirSubscriptions",
                column: "QueuesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOrdersAndTheirSubscriptions");

            migrationBuilder.DropTable(
                name: "UserQueuesAndTheirSubscriptions");

            migrationBuilder.DropTable(
                name: "OrderSubscriptions");

            migrationBuilder.DropTable(
                name: "QueueSubscriptions");

            migrationBuilder.DropColumn(
                name: "OrderSubscriptionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QueueSubscriptionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssignmentDate",
                table: "Queues");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Queues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "Queues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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
                name: "IX_Queues_CreationDate",
                table: "Queues",
                column: "CreationDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreationDate",
                table: "Orders",
                column: "CreationDate",
                descending: new bool[0]);

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
