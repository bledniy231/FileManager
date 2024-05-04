using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_CourseTypes_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.DropTable(
                name: "CourseTypes",
                schema: "PianoMentor");

            migrationBuilder.CreateTable(
                name: "CourseItemTypes",
                schema: "PianoMentor",
                columns: table => new
                {
                    CourseItemTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItemTypes", x => x.CourseItemTypeId);
                });

            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "CourseItemTypes",
                columns: new[] { "CourseItemTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Lecture" },
                    { 2, "Exercise" },
                    { 3, "Quiz" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "CourseTypeId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItemTypes",
                principalColumn: "CourseItemTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.DropTable(
                name: "CourseItemTypes",
                schema: "PianoMentor");

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

            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "CourseTypes",
                columns: new[] { "CourseTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Lecture" },
                    { 2, "Exercise" },
                    { 3, "Quiz" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_CourseTypes_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "CourseTypeId",
                principalSchema: "PianoMentor",
                principalTable: "CourseTypes",
                principalColumn: "CourseTypeId");
        }
    }
}
