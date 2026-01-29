using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wellbeing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWellbeingSubDimensionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WellbeingSubDimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    WellbeingDimensionId = table.Column<int>(type: "integer", nullable: false),
                    ClientsId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellbeingSubDimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellbeingSubDimensions_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WellbeingSubDimensions_WellbeingDimensions_WellbeingDimensi~",
                        column: x => x.WellbeingDimensionId,
                        principalTable: "WellbeingDimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WellbeingSubDimensions_ClientsId",
                table: "WellbeingSubDimensions",
                column: "ClientsId");

            migrationBuilder.CreateIndex(
                name: "IX_WellbeingSubDimensions_WellbeingDimensionId",
                table: "WellbeingSubDimensions",
                column: "WellbeingDimensionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WellbeingSubDimensions");
        }
    }
}
