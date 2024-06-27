using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_queue_job_outbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "queue_job_outbox_messages",
                columns: table => new
                {
                    queue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_id = table.Column<string>(type: "text", nullable: false),
                    occured_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_queue_job_outbox_messages", x => x.queue_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_queue_job_outbox_messages_processed_on_utc",
                table: "queue_job_outbox_messages",
                column: "processed_on_utc",
                filter: "processed_on_utc is not null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "queue_job_outbox_messages");
        }
    }
}
