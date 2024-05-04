using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "PianoMentor",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "CourseTypes",
                schema: "PianoMentor",
                columns: table => new
                {
                    CourseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTypes", x => x.CourseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CourseItems",
                schema: "PianoMentor",
                columns: table => new
                {
                    CourseItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CourseTypeId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItems", x => x.CourseItemId);
                    table.ForeignKey(
                        name: "FK_CourseItems_CourseTypes_CourseTypeId",
                        column: x => x.CourseTypeId,
                        principalSchema: "PianoMentor",
                        principalTable: "CourseTypes",
                        principalColumn: "CourseTypeId");
                    table.ForeignKey(
                        name: "FK_CourseItems_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "PianoMentor",
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "CourseTypes",
                columns: new[] { "CourseTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Lesson" },
                    { 2, "Exercise" },
                    { 3, "Quiz" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CourseId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "CourseTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseItems",
                schema: "PianoMentor");

            migrationBuilder.DropTable(
                name: "CourseTypes",
                schema: "PianoMentor");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "PianoMentor");
        }
    }
}
