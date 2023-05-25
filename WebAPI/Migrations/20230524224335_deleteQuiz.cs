using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class deleteQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizEntityQuizItemEntity");

            migrationBuilder.DropTable(
                name: "QuizItemAnswerEntityQuizItemEntity");

            migrationBuilder.DropTable(
                name: "QuizItemUserAnswerEntity");

            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "QuizEntity");

            migrationBuilder.DropTable(
                name: "QuizItemAnswerEntity");

            migrationBuilder.DropTable(
                name: "QuizItemEntity");

            migrationBuilder.DropTable(
                name: "QuizItems");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizItemAnswerEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItemAnswerEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizItemEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItemEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizEntityQuizItemEntity",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false),
                    QuizzesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizEntityQuizItemEntity", x => new { x.ItemsId, x.QuizzesId });
                    table.ForeignKey(
                        name: "FK_QuizEntityQuizItemEntity_QuizEntity_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "QuizEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizEntityQuizItemEntity_QuizItemEntity_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "QuizItemEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizItemAnswerEntityQuizItemEntity",
                columns: table => new
                {
                    IncorrectAnswersId = table.Column<int>(type: "int", nullable: false),
                    QuizItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItemAnswerEntityQuizItemEntity", x => new { x.IncorrectAnswersId, x.QuizItemsId });
                    table.ForeignKey(
                        name: "FK_QuizItemAnswerEntityQuizItemEntity_QuizItemAnswerEntity_IncorrectAnswersId",
                        column: x => x.IncorrectAnswersId,
                        principalTable: "QuizItemAnswerEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizItemAnswerEntityQuizItemEntity_QuizItemEntity_QuizItemsId",
                        column: x => x.QuizItemsId,
                        principalTable: "QuizItemEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizItemUserAnswerEntity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    QuizItemId = table.Column<int>(type: "int", nullable: false),
                    UserAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItemUserAnswerEntity", x => new { x.UserId, x.QuizId, x.QuizItemId });
                    table.ForeignKey(
                        name: "FK_QuizItemUserAnswerEntity_QuizItemEntity_QuizItemId",
                        column: x => x.QuizItemId,
                        principalTable: "QuizItemEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizItems_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizItemId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_QuizItems_QuizItemId",
                        column: x => x.QuizItemId,
                        principalTable: "QuizItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "QuizEntity",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Matematyka" },
                    { 2, "Arytmetyka" }
                });

            migrationBuilder.InsertData(
                table: "QuizItemAnswerEntity",
                columns: new[] { "Id", "Answer" },
                values: new object[,]
                {
                    { 1, "1" },
                    { 2, "2" },
                    { 3, "3" },
                    { 4, "4" },
                    { 5, "5" },
                    { 6, "6" },
                    { 7, "7" },
                    { 8, "8" },
                    { 9, "9" },
                    { 10, "0" }
                });

            migrationBuilder.InsertData(
                table: "QuizItemEntity",
                columns: new[] { "Id", "CorrectAnswer", "Question" },
                values: new object[,]
                {
                    { 1, "5", "2 + 3" },
                    { 2, "6", "2 * 3" },
                    { 3, "5", "8 - 3" },
                    { 4, "4", "8 : 2" }
                });

            migrationBuilder.InsertData(
                table: "QuizEntityQuizItemEntity",
                columns: new[] { "ItemsId", "QuizzesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "QuizItemAnswerEntityQuizItemEntity",
                columns: new[] { "IncorrectAnswersId", "QuizItemsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 4 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 4, 2 },
                    { 6, 4 },
                    { 7, 2 },
                    { 8, 4 },
                    { 9, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizEntityQuizItemEntity_QuizzesId",
                table: "QuizEntityQuizItemEntity",
                column: "QuizzesId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItemAnswerEntityQuizItemEntity_QuizItemsId",
                table: "QuizItemAnswerEntityQuizItemEntity",
                column: "QuizItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_QuizId",
                table: "QuizItems",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItemUserAnswerEntity_QuizItemId",
                table: "QuizItemUserAnswerEntity",
                column: "QuizItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuizItemId",
                table: "UserAnswers",
                column: "QuizItemId");
        }
    }
}
