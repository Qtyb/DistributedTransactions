using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductApi.Data.Migrations
{
    public partial class OutboxMessage_Error_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "OutboxMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "OutboxMessages");
        }
    }
}
