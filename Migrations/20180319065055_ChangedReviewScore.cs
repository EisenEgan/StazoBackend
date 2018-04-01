using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Stazo.API.Migrations
{
    public partial class ChangedReviewScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Star_StarId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Star");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_StarId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "StarId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "StarScore",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StarScore",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "StarId",
                table: "Reviews",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Star",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Star", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_StarId",
                table: "Reviews",
                column: "StarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Star_StarId",
                table: "Reviews",
                column: "StarId",
                principalTable: "Star",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
