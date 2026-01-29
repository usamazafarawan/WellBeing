using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wellbeing.Infrastructure.Migrations
{
    public partial class RenameTablesToClientsAndAspNetUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Companies_CompanyId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompanyId",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Clients",
                schema: null);

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "AspNetUsers",
                schema: null);

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "AspNetUsers",
                newName: "ClientsId");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_Email",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Email");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientsId",
                table: "AspNetUsers",
                column: "ClientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clients_ClientsId",
                table: "AspNetUsers",
                column: "ClientsId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clients_ClientsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClientsId",
                table: "AspNetUsers");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                newName: "IX_Clients_Email");

            migrationBuilder.RenameColumn(
                name: "ClientsId",
                table: "AspNetUsers",
                newName: "CompanyId");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Clients",
                schema: null);

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Companies",
                schema: null);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId",
                table: "Clients",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Companies_CompanyId",
                table: "Clients",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
