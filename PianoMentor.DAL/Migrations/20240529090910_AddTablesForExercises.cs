using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesForExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseTypes",
                schema: "PianoMentor",
                columns: table => new
                {
                    ExerciseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTypes", x => x.ExerciseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Intervals",
                schema: "PianoMentor",
                columns: table => new
                {
                    IntervalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntervalName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervals", x => x.IntervalId);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseTasks",
                schema: "PianoMentor",
                columns: table => new
                {
                    ExerciseTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseItemId = table.Column<int>(type: "int", nullable: false),
                    ExerciseTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTasks", x => x.ExerciseTaskId);
                    table.ForeignKey(
                        name: "FK_ExerciseTasks_CourseItems_CourseItemId",
                        column: x => x.CourseItemId,
                        principalSchema: "PianoMentor",
                        principalTable: "CourseItems",
                        principalColumn: "CourseItemId");
                    table.ForeignKey(
                        name: "FK_ExerciseTasks_ExerciseTypes_ExerciseTypeId",
                        column: x => x.ExerciseTypeId,
                        principalSchema: "PianoMentor",
                        principalTable: "ExerciseTypes",
                        principalColumn: "ExerciseTypeId");
                });

            migrationBuilder.CreateTable(
                name: "IntervalsInTasks",
                schema: "PianoMentor",
                columns: table => new
                {
                    ExerciseTaskId = table.Column<int>(type: "int", nullable: false),
                    IntervalsInTaskIntervalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervalsInTasks", x => new { x.ExerciseTaskId, x.IntervalsInTaskIntervalId });
                    table.ForeignKey(
                        name: "FK_IntervalsInTasks_ExerciseTasks_ExerciseTaskId",
                        column: x => x.ExerciseTaskId,
                        principalSchema: "PianoMentor",
                        principalTable: "ExerciseTasks",
                        principalColumn: "ExerciseTaskId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntervalsInTasks_Intervals_IntervalsInTaskIntervalId",
                        column: x => x.IntervalsInTaskIntervalId,
                        principalSchema: "PianoMentor",
                        principalTable: "Intervals",
                        principalColumn: "IntervalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "ExerciseTypes",
                columns: new[] { "ExerciseTypeId", "ExerciseTypeName" },
                values: new object[,]
                {
                    { 1, "ComparisonAsc" },
                    { 2, "ComparisonDesc" },
                    { 3, "DeterminationAsc" },
                    { 4, "DeterminationDesc" },
                    { 5, "ComparisonHarmonious" },
                    { 6, "DeterminationHarmonious" },
                    { 7, "DeterminationMultiple" }
                });

            migrationBuilder.InsertData(
                schema: "PianoMentor",
                table: "Intervals",
                columns: new[] { "IntervalId", "IntervalName" },
                values: new object[,]
                {
                    { 1, "MinorSecond" },
                    { 2, "MajorSecond" },
                    { 3, "MinorThird" },
                    { 4, "MajorThird" },
                    { 5, "PerfectFourth" },
                    { 6, "Tritone" },
                    { 7, "PerfectFifth" },
                    { 8, "MinorSixth" },
                    { 9, "MajorSixth" },
                    { 10, "MinorSeventh" },
                    { 11, "MajorSeventh" },
                    { 12, "Octave" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTasks_CourseItemId",
                schema: "PianoMentor",
                table: "ExerciseTasks",
                column: "CourseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTasks_ExerciseTypeId",
                schema: "PianoMentor",
                table: "ExerciseTasks",
                column: "ExerciseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntervalsInTasks_IntervalsInTaskIntervalId",
                schema: "PianoMentor",
                table: "IntervalsInTasks",
                column: "IntervalsInTaskIntervalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntervalsInTasks",
                schema: "PianoMentor");

            migrationBuilder.DropTable(
                name: "ExerciseTasks",
                schema: "PianoMentor");

            migrationBuilder.DropTable(
                name: "Intervals",
                schema: "PianoMentor");

            migrationBuilder.DropTable(
                name: "ExerciseTypes",
                schema: "PianoMentor");
        }
    }
}
