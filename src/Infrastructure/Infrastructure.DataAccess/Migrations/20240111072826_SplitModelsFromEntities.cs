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
            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    ActiveFrom = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ActiveUntil = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    AssignmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramId = table.Column<string>(type: "text", nullable: false),
                    Fullname = table.Column<string>(type: "text", nullable: false),
                    RegistrationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    QueueSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QueueId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    Ready = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    OrderSubscriptionModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrdersAndTheirSubscriptions", x => new { x.OrderSubscriptionModelId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_UserOrdersAndTheirSubscriptions_OrderSubscriptions_OrderSub~",
                        column: x => x.OrderSubscriptionModelId,
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
                    QueueSubscriptionModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    QueuesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQueuesAndTheirSubscriptions", x => new { x.QueueSubscriptionModelId, x.QueuesId });
                    table.ForeignKey(
                        name: "FK_UserQueuesAndTheirSubscriptions_QueueSubscriptions_QueueSub~",
                        column: x => x.QueueSubscriptionModelId,
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
                name: "IX_Orders_QueueId",
                table: "Orders",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

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
                name: "Orders");

            migrationBuilder.DropTable(
                name: "QueueSubscriptions");

            migrationBuilder.DropTable(
                name: "Queues");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
