using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddViewPagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViewPagerTexts",
                schema: "FileManager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewPagerTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViewPagerTextNumberRanges",
                schema: "FileManager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewPagerTextId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewPagerTextNumberRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewPagerTextNumberRanges_ViewPagerTexts_ViewPagerTextId",
                        column: x => x.ViewPagerTextId,
                        principalSchema: "FileManager",
                        principalTable: "ViewPagerTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewPagerTextNumberRanges_ViewPagerTextId",
                schema: "FileManager",
                table: "ViewPagerTextNumberRanges",
                column: "ViewPagerTextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewPagerTextNumberRanges",
                schema: "FileManager");

            migrationBuilder.DropTable(
                name: "ViewPagerTexts",
                schema: "FileManager");
        }
    }
}
