using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "PianoMentor",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "AttachedDataSetId",
                schema: "PianoMentor",
                table: "CourseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "CourseItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "PianoMentor",
                table: "CourseItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_AttachedDataSetId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "AttachedDataSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseItems_DataSets_AttachedDataSetId",
                schema: "PianoMentor",
                table: "CourseItems",
                column: "AttachedDataSetId",
                principalSchema: "PianoMentor",
                principalTable: "DataSets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseItems_DataSets_AttachedDataSetId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.DropIndex(
                name: "IX_CourseItems_AttachedDataSetId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "PianoMentor",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "AttachedDataSetId",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "CourseItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "PianoMentor",
                table: "CourseItems");
        }
    }
}
