using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNameInCourseItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.RenameColumn(
                name: "CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                newName: "CourseItemTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItems_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                newName: "IX_CourseItems_CourseItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseItemTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "CourseItemTypeId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItemTypes",
                principalColumn: "CourseItemTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseItemTypeId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.RenameColumn(
                name: "CourseItemTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                newName: "CourseTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItems_CourseItemTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                newName: "IX_CourseItems_CourseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_CourseItemTypes_CourseTypeId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "CourseTypeId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItemTypes",
                principalColumn: "CourseItemTypeId");
        }
    }
}
