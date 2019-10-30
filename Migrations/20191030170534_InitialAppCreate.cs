using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestYourself.Migrations
{
    public partial class InitialAppCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    VocabularyCount = table.Column<int>(nullable: false),
                    RatePosition = table.Column<int>(nullable: false),
                    LocationCity = table.Column<string>(nullable: true),
                    LocationCountry = table.Column<string>(nullable: true),
                    TotalWordsCount = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyValues",
                columns: table => new
                {
                    VocabularyValuesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Word = table.Column<string>(nullable: true),
                    WordTranslation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyValues", x => x.VocabularyValuesId);
                });

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    UserRatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => x.UserRatingId);
                    table.ForeignKey(
                        name: "FK_UserRatings_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vocabularies",
                columns: table => new
                {
                    VocabularyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsOfficial = table.Column<bool>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: true),
                    VocabularyValuesId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocabularies", x => x.VocabularyId);
                    table.ForeignKey(
                        name: "FK_Vocabularies_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vocabularies_VocabularyValues_VocabularyValuesId",
                        column: x => x.VocabularyValuesId,
                        principalTable: "VocabularyValues",
                        principalColumn: "VocabularyValuesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyRatings",
                columns: table => new
                {
                    VocabularyRatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VocabularyCaption = table.Column<string>(nullable: true),
                    VocabularyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyRatings", x => x.VocabularyRatingId);
                    table.ForeignKey(
                        name: "FK_VocabularyRatings_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "VocabularyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ProfileId",
                table: "UserRatings",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_ProfileId",
                table: "Vocabularies",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Vocabularies_VocabularyValuesId",
                table: "Vocabularies",
                column: "VocabularyValuesId");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyRatings_VocabularyId",
                table: "VocabularyRatings",
                column: "VocabularyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Vocabularies_UserId",
                table: "AspNetUsers",
                column: "UserId",
                principalTable: "Vocabularies",
                principalColumn: "VocabularyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Vocabularies_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.DropTable(
                name: "VocabularyRatings");

            migrationBuilder.DropTable(
                name: "Vocabularies");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "VocabularyValues");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");
        }
    }
}
