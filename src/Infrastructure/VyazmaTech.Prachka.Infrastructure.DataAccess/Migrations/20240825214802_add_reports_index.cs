using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_reports_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            """
            create index concurrently if not exists ix_orders_creation_date_time on orders(creation_date_time)
            include (user_id, status, creation_date_time, price)
            where (status != 0);
            """, suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            """
            drop index concurrently if exists ix_orders_creation_date_time;
            """, suppressTransaction: true);
        }
    }
}
