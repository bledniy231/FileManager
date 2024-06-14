using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBinaryType_AddIsDeletedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "DataSets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BinaryTypeId",
                schema: "PianoMentor",
                table: "Binaries",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "Binaries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BinaryTypes",
                schema: "PianoMentor",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinaryTypes", x => x.TypeId);
                });

            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "BinaryTypes",
                columns: new[] { "TypeId", "TypeName" },
                values: new object[,]
                {
                    { 1, "UserFile" },
                    { 2, "UserProfilePhoto" },
                    { 3, "CourseItemFile" },
                    { 4, "QuizQuestionFile" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Binaries_BinaryTypeId",
                schema: "PianoMentor",
                table: "Binaries",
                column: "BinaryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Binaries_BinaryTypes_BinaryTypeId",
                schema: "PianoMentor",
                table: "Binaries",
                column: "BinaryTypeId",
                principalSchema: "PianoMentor",
                principalTable: "BinaryTypes",
                principalColumn: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Binaries_BinaryTypes_BinaryTypeId",
                schema: "PianoMentor",
                table: "Binaries");

            migrationBuilder.DropTable(
                name: "BinaryTypes",
                schema: "PianoMentor");

            migrationBuilder.DropIndex(
                name: "IX_Binaries_BinaryTypeId",
                schema: "PianoMentor",
                table: "Binaries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "DataSets");

            migrationBuilder.DropColumn(
                name: "BinaryTypeId",
                schema: "PianoMentor",
                table: "Binaries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "PianoMentor",
                table: "Binaries");
        }
    }
}
