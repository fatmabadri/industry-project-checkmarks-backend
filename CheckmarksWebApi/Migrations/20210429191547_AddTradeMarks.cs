using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckmarksWebApi.Migrations
{
    public partial class AddTradeMarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trademarks",
                columns: table => new
                {
                    ApplicationNumber = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    FileDate = table.Column<DateTime>(nullable: false),
                    RegDate = table.Column<DateTime>(nullable: false),
                    IntrnlRenewDate = table.Column<DateTime>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    StatusDescEn = table.Column<string>(nullable: true),
                    NiceClasses = table.Column<string>(nullable: true),
                    TmType = table.Column<string>(nullable: true),
                    ApplicationNumberL = table.Column<string>(nullable: true),
                    MediaUrls = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trademarks", x => x.ApplicationNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trademarks");
        }
    }
}
