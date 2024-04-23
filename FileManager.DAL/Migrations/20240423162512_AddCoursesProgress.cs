using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FileManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCoursesProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "FileManager",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CourseItemsProgressTypes",
                schema: "FileManager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItemsProgressTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersCoursesProgresses",
                schema: "FileManager",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ProgressInPercent = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCoursesProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersCoursesProgresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "FileManager",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersCoursesProgresses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "FileManager",
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersCoursesItemsProgresses",
                schema: "FileManager",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CourseItemId = table.Column<int>(type: "int", nullable: false),
                    CourseItemProgressTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCoursesItemsProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersCoursesItemsProgresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "FileManager",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersCoursesItemsProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                        column: x => x.CourseItemProgressTypeId,
                        principalSchema: "FileManager",
                        principalTable: "CourseItemsProgressTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersCoursesItemsProgresses_CourseItems_CourseItemId",
                        column: x => x.CourseItemId,
                        principalSchema: "FileManager",
                        principalTable: "CourseItems",
                        principalColumn: "CourseItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "FileManager",
                table: "CourseItemsProgressTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "NotStarted" },
                    { 2, "InProgress" },
                    { 3, "Completed" },
                    { 4, "Failed" }
                });

            migrationBuilder.UpdateData(
                schema: "FileManager",
                table: "CourseTypes",
                keyColumn: "CourseTypeId",
                keyValue: 1,
                column: "Name",
                value: "Lecture");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCoursesItemsProgresses_CourseItemId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "CourseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCoursesItemsProgresses_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "CourseItemProgressTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCoursesItemsProgresses_UserId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCoursesProgresses_CourseId",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCoursesProgresses_UserId",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersCoursesItemsProgresses",
                schema: "FileManager");

            migrationBuilder.DropTable(
                name: "UsersCoursesProgresses",
                schema: "FileManager");

            migrationBuilder.DropTable(
                name: "CourseItemsProgressTypes",
                schema: "FileManager");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "FileManager",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                schema: "FileManager",
                table: "CourseTypes",
                keyColumn: "CourseTypeId",
                keyValue: 1,
                column: "Name",
                value: "Lesson");
        }
    }
}
