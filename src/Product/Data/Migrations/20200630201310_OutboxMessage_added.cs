using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductApi.Data.Migrations
{
    public partial class OutboxMessage_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(nullable: true),
                    HttpMethod = table.Column<byte>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    OccurredOn = table.Column<DateTime>(nullable: false),
                    ProcessedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages");
        }
    }
}
