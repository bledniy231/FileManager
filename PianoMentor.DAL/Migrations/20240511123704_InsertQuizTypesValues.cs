using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InsertQuizTypesValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "QuizQuestionsTypes",
                columns: new[] { "QuizQuestionTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "SingleChoice" },
                    { 2, "MultipleChoice" },
                    { 3, "FreeText" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "PianoMentor",
                table: "QuizQuestionsTypes",
                keyColumn: "QuizQuestionTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "PianoMentor",
                table: "QuizQuestionsTypes",
                keyColumn: "QuizQuestionTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "PianoMentor",
                table: "QuizQuestionsTypes",
                keyColumn: "QuizQuestionTypeId",
                keyValue: 3);
        }
    }
}
