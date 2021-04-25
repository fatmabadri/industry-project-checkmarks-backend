using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckmarksWebApi.Migrations
{
    public partial class perrycreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NICEClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NICEClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NICETerms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NICETerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NICETerms_NICEClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "NICEClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NICETerms_ClassId",
                table: "NICETerms",
                column: "ClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NICETerms");

            migrationBuilder.DropTable(
                name: "NICEClasses");
        }
    }
}
