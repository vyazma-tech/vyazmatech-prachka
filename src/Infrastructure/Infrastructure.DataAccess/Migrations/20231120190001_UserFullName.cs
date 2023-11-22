using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Queues_QueueEntityId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Subscriptions_SubscriberEntityId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_QueueEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QueueEntityId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "SubscriberEntityId",
                table: "Orders",
                newName: "SubscriptionEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SubscriberEntityId",
                table: "Orders",
                newName: "IX_Orders_SubscriptionEntityId");

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "QueueId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_QueueId",
                table: "Orders",
                column: "QueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Queues_QueueId",
                table: "Orders",
                column: "QueueId",
                principalTable: "Queues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Subscriptions_SubscriptionEntityId",
                table: "Orders",
                column: "SubscriptionEntityId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Queues_QueueId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Subscriptions_SubscriptionEntityId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_QueueId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QueueId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "SubscriptionEntityId",
                table: "Orders",
                newName: "SubscriberEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SubscriptionEntityId",
                table: "Orders",
                newName: "IX_Orders_SubscriberEntityId");

            migrationBuilder.AddColumn<Guid>(
                name: "QueueEntityId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_QueueEntityId",
                table: "Orders",
                column: "QueueEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Queues_QueueEntityId",
                table: "Orders",
                column: "QueueEntityId",
                principalTable: "Queues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Subscriptions_SubscriberEntityId",
                table: "Orders",
                column: "SubscriberEntityId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
