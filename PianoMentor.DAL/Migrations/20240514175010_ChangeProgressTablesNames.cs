using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProgressTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesItemsProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItems_CourseItemId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "UsersCoursesProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesProgresses_Courses_CourseId",
                schema: "FileManager",
                table: "UsersCoursesProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCoursesProgresses",
                schema: "FileManager",
                table: "UsersCoursesProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCoursesItemsProgresses",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.RenameTable(
                name: "UsersCoursesProgresses",
                schema: "FileManager",
                newName: "CourseUsersProgresses",
                newSchema: "FileManager");

            migrationBuilder.RenameTable(
                name: "UsersCoursesItemsProgresses",
                schema: "FileManager",
                newName: "CourseItemsUsersProgresses",
                newSchema: "FileManager");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesProgresses_UserId",
                schema: "FileManager",
                table: "CourseUsersProgresses",
                newName: "IX_CourseUsersProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesProgresses_CourseId",
                schema: "FileManager",
                table: "CourseUsersProgresses",
                newName: "IX_CourseUsersProgresses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesItemsProgresses_UserId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                newName: "IX_CourseItemsUsersProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesItemsProgresses_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                newName: "IX_CourseItemsUsersProgresses_CourseItemProgressTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesItemsProgresses_CourseItemId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                newName: "IX_CourseItemsUsersProgresses_CourseItemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseUsersProgresses",
                schema: "FileManager",
                table: "CourseUsersProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseItemsUsersProgresses",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItemsUsersProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                column: "UserId",
                principalSchema: "FileManager",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                column: "CourseItemProgressTypeId",
                principalSchema: "FileManager",
                principalTable: "CourseItemsProgressTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItems_CourseItemId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses",
                column: "CourseItemId",
                principalSchema: "FileManager",
                principalTable: "CourseItems",
                principalColumn: "CourseItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUsersProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "CourseUsersProgresses",
                column: "UserId",
                principalSchema: "FileManager",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUsersProgresses_Courses_CourseId",
                schema: "FileManager",
                table: "CourseUsersProgresses",
                column: "CourseId",
                principalSchema: "FileManager",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItemsUsersProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItems_CourseItemId",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUsersProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "CourseUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUsersProgresses_Courses_CourseId",
                schema: "FileManager",
                table: "CourseUsersProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseUsersProgresses",
                schema: "FileManager",
                table: "CourseUsersProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseItemsUsersProgresses",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "FileManager",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.RenameTable(
                name: "CourseUsersProgresses",
                schema: "FileManager",
                newName: "UsersCoursesProgresses",
                newSchema: "FileManager");

            migrationBuilder.RenameTable(
                name: "CourseItemsUsersProgresses",
                schema: "FileManager",
                newName: "UsersCoursesItemsProgresses",
                newSchema: "FileManager");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUsersProgresses_UserId",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                newName: "IX_UsersCoursesProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUsersProgresses_CourseId",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                newName: "IX_UsersCoursesProgresses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItemsUsersProgresses_UserId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                newName: "IX_UsersCoursesItemsProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItemsUsersProgresses_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                newName: "IX_UsersCoursesItemsProgresses_CourseItemProgressTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItemsUsersProgresses_CourseItemId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                newName: "IX_UsersCoursesItemsProgresses_CourseItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCoursesProgresses",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCoursesItemsProgresses",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesItemsProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "UserId",
                principalSchema: "FileManager",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "CourseItemProgressTypeId",
                principalSchema: "FileManager",
                principalTable: "CourseItemsProgressTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItems_CourseItemId",
                schema: "FileManager",
                table: "UsersCoursesItemsProgresses",
                column: "CourseItemId",
                principalSchema: "FileManager",
                principalTable: "CourseItems",
                principalColumn: "CourseItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesProgresses_AspNetUsers_UserId",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                column: "UserId",
                principalSchema: "FileManager",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesProgresses_Courses_CourseId",
                schema: "FileManager",
                table: "UsersCoursesProgresses",
                column: "CourseId",
                principalSchema: "FileManager",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
