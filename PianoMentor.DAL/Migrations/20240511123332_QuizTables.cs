﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PianoMentor.DAL.Migrations
{
    /// <inheritdoc />
    public partial class QuizTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseItems_AttachedDataSetId",
                schema: "FileManager",
                table: "CourseItems");

            migrationBuilder.CreateTable(
                name: "QuizQuestionsTypes",
                schema: "FileManager",
                columns: table => new
                {
                    QuizQuestionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionsTypes", x => x.QuizQuestionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                schema: "FileManager",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AttachedDataSetId = table.Column<long>(type: "bigint", nullable: true),
                    CourseItemId = table.Column<int>(type: "int", nullable: false),
                    QuizQuestionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_CourseItems_CourseItemId",
                        column: x => x.CourseItemId,
                        principalSchema: "FileManager",
                        principalTable: "CourseItems",
                        principalColumn: "CourseItemId");
                    table.ForeignKey(
                        name: "FK_QuizQuestions_DataSets_AttachedDataSetId",
                        column: x => x.AttachedDataSetId,
                        principalSchema: "FileManager",
                        principalTable: "DataSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizQuestions_QuizQuestionsTypes_QuizQuestionTypeId",
                        column: x => x.QuizQuestionTypeId,
                        principalSchema: "FileManager",
                        principalTable: "QuizQuestionsTypes",
                        principalColumn: "QuizQuestionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestionsAnswers",
                schema: "FileManager",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    QuizQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionsAnswers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_QuizQuestionsAnswers_QuizQuestions_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalSchema: "FileManager",
                        principalTable: "QuizQuestions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestionsUsersAnswersLogs",
                schema: "FileManager",
                columns: table => new
                {
                    AnswerLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    UserAnswerText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionsUsersAnswersLogs", x => x.AnswerLogId);
                    table.ForeignKey(
                        name: "FK_QuizQuestionsUsersAnswersLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "FileManager",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizQuestionsUsersAnswersLogs_QuizQuestionsAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalSchema: "FileManager",
                        principalTable: "QuizQuestionsAnswers",
                        principalColumn: "AnswerId");
                    table.ForeignKey(
                        name: "FK_QuizQuestionsUsersAnswersLogs_QuizQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "FileManager",
                        principalTable: "QuizQuestions",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_AttachedDataSetId",
                schema: "FileManager",
                table: "CourseItems",
                column: "AttachedDataSetId",
                unique: true,
                filter: "[AttachedDataSetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_AttachedDataSetId",
                schema: "FileManager",
                table: "QuizQuestions",
                column: "AttachedDataSetId",
                unique: true,
                filter: "[AttachedDataSetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_CourseItemId",
                schema: "FileManager",
                table: "QuizQuestions",
                column: "CourseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizQuestionTypeId",
                schema: "FileManager",
                table: "QuizQuestions",
                column: "QuizQuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionsAnswers_QuizQuestionId",
                schema: "FileManager",
                table: "QuizQuestionsAnswers",
                column: "QuizQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionsUsersAnswersLogs_AnswerId",
                schema: "FileManager",
                table: "QuizQuestionsUsersAnswersLogs",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionsUsersAnswersLogs_QuestionId",
                schema: "FileManager",
                table: "QuizQuestionsUsersAnswersLogs",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionsUsersAnswersLogs_UserId",
                schema: "FileManager",
                table: "QuizQuestionsUsersAnswersLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizQuestionsUsersAnswersLogs",
                schema: "FileManager");

            migrationBuilder.DropTable(
                name: "QuizQuestionsAnswers",
                schema: "FileManager");

            migrationBuilder.DropTable(
                name: "QuizQuestions",
                schema: "FileManager");

            migrationBuilder.DropTable(
                name: "QuizQuestionsTypes",
                schema: "FileManager");

            migrationBuilder.DropIndex(
                name: "IX_CourseItems_AttachedDataSetId",
                schema: "FileManager",
                table: "CourseItems");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_AttachedDataSetId",
                schema: "FileManager",
                table: "CourseItems",
                column: "AttachedDataSetId");
        }
    }
}
