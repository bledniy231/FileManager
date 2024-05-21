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
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItems_CourseItemId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCoursesProgresses_Courses_CourseId",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCoursesProgresses",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCoursesItemsProgresses",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses");

            migrationBuilder.RenameTable(
                name: "UsersCoursesProgresses",
                schema: "PianoMentor",
                newName: "CourseUsersProgresses",
                newSchema: "PianoMentor");

            migrationBuilder.RenameTable(
                name: "UsersCoursesItemsProgresses",
                schema: "PianoMentor",
                newName: "CourseItemsUsersProgresses",
                newSchema: "PianoMentor");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesProgresses_UserId",
                schema: "PianoMentor",
                table: "CourseUsersProgresses",
                newName: "IX_CourseUsersProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesProgresses_CourseId",
                schema: "PianoMentor",
                table: "CourseUsersProgresses",
                newName: "IX_CourseUsersProgresses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesItemsProgresses_UserId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                newName: "IX_CourseItemsUsersProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesItemsProgresses_CourseItemProgressTypeId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                newName: "IX_CourseItemsUsersProgresses_CourseItemProgressTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCoursesItemsProgresses_CourseItemId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                newName: "IX_CourseItemsUsersProgresses_CourseItemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseUsersProgresses",
                schema: "PianoMentor",
                table: "CourseUsersProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseItemsUsersProgresses",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItemsUsersProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                column: "UserId",
                principalSchema: "PianoMentor",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                column: "CourseItemProgressTypeId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItemsProgressTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItems_CourseItemId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses",
                column: "CourseItemId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItems",
                principalColumn: "CourseItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUsersProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "CourseUsersProgresses",
                column: "UserId",
                principalSchema: "PianoMentor",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUsersProgresses_Courses_CourseId",
                schema: "PianoMentor",
                table: "CourseUsersProgresses",
                column: "CourseId",
                principalSchema: "PianoMentor",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItemsUsersProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseItemsUsersProgresses_CourseItems_CourseItemId",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUsersProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "CourseUsersProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUsersProgresses_Courses_CourseId",
                schema: "PianoMentor",
                table: "CourseUsersProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseUsersProgresses",
                schema: "PianoMentor",
                table: "CourseUsersProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseItemsUsersProgresses",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "PianoMentor",
                table: "CourseItemsUsersProgresses");

            migrationBuilder.RenameTable(
                name: "CourseUsersProgresses",
                schema: "PianoMentor",
                newName: "UsersCoursesProgresses",
                newSchema: "PianoMentor");

            migrationBuilder.RenameTable(
                name: "CourseItemsUsersProgresses",
                schema: "PianoMentor",
                newName: "UsersCoursesItemsProgresses",
                newSchema: "PianoMentor");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUsersProgresses_UserId",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses",
                newName: "IX_UsersCoursesProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUsersProgresses_CourseId",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses",
                newName: "IX_UsersCoursesProgresses_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItemsUsersProgresses_UserId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                newName: "IX_UsersCoursesItemsProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItemsUsersProgresses_CourseItemProgressTypeId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                newName: "IX_UsersCoursesItemsProgresses_CourseItemProgressTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseItemsUsersProgresses_CourseItemId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                newName: "IX_UsersCoursesItemsProgresses_CourseItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCoursesProgresses",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCoursesItemsProgresses",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesItemsProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                column: "UserId",
                principalSchema: "PianoMentor",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItemsProgressTypes_CourseItemProgressTypeId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                column: "CourseItemProgressTypeId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItemsProgressTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesItemsProgresses_CourseItems_CourseItemId",
                schema: "PianoMentor",
                table: "UsersCoursesItemsProgresses",
                column: "CourseItemId",
                principalSchema: "PianoMentor",
                principalTable: "CourseItems",
                principalColumn: "CourseItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesProgresses_AspNetUsers_UserId",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses",
                column: "UserId",
                principalSchema: "PianoMentor",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCoursesProgresses_Courses_CourseId",
                schema: "PianoMentor",
                table: "UsersCoursesProgresses",
                column: "CourseId",
                principalSchema: "PianoMentor",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
