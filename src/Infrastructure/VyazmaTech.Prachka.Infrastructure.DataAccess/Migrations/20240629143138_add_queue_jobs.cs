using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_queue_jobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "queue_job_messages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    queue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_queue_job_messages", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_queue_job_messages_queue_id_job_id",
                table: "queue_job_messages",
                columns: new[] { "queue_id", "job_id" },
                unique: true)
                .Annotation("Npgsql:CreatedConcurrently", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "queue_job_messages");
        }
    }
}
