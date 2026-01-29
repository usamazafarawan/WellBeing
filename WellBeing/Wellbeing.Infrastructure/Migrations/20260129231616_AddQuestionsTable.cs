using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wellbeing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionText = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    QuestionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WellbeingDimensionId = table.Column<int>(type: "integer", nullable: false),
                    WellbeingSubDimensionId = table.Column<int>(type: "integer", nullable: false),
                    ClientsId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_WellbeingDimensions_WellbeingDimensionId",
                        column: x => x.WellbeingDimensionId,
                        principalTable: "WellbeingDimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_WellbeingSubDimensions_WellbeingSubDimensionId",
                        column: x => x.WellbeingSubDimensionId,
                        principalTable: "WellbeingSubDimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ClientsId",
                table: "Questions",
                column: "ClientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_WellbeingDimensionId",
                table: "Questions",
                column: "WellbeingDimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_WellbeingSubDimensionId",
                table: "Questions",
                column: "WellbeingSubDimensionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
