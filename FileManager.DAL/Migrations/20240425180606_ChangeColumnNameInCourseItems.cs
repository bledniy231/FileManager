using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNameInCourseItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseTypeId",
                schema: "FileManager",
                table: "CourseItems");

            migrationBuilder.RenameColumn(
                name: "CourseTypeId",
                schema: "FileManager",
                table: "CourseItems",
                newName: "CourseItemTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItems_CourseTypeId",
                schema: "FileManager",
                table: "CourseItems",
                newName: "IX_CourseItems_CourseItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseItemTypeId",
                schema: "FileManager",
                table: "CourseItems",
                column: "CourseItemTypeId",
                principalSchema: "FileManager",
                principalTable: "CourseItemTypes",
                principalColumn: "CourseItemTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseItemTypeId",
                schema: "FileManager",
                table: "CourseItems");

            migrationBuilder.RenameColumn(
                name: "CourseItemTypeId",
                schema: "FileManager",
                table: "CourseItems",
                newName: "CourseTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItems_CourseItemTypeId",
                schema: "FileManager",
                table: "CourseItems",
                newName: "IX_CourseItems_CourseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseTypeId",
                schema: "FileManager",
                table: "CourseItems",
                column: "CourseTypeId",
                principalSchema: "FileManager",
                principalTable: "CourseItemTypes",
                principalColumn: "CourseItemTypeId");
        }
    }
}
