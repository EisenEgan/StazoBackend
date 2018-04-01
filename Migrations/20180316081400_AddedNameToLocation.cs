using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Stazo.API.Migrations
{
    public partial class AddedNameToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationProduct_Locations_LocationId",
                table: "LocationProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationProduct_Products_ProductId",
                table: "LocationProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationProduct",
                table: "LocationProduct");

            migrationBuilder.RenameTable(
                name: "LocationProduct",
                newName: "LocationProducts");

            migrationBuilder.RenameIndex(
                name: "IX_LocationProduct_ProductId",
                table: "LocationProducts",
                newName: "IX_LocationProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationProduct_LocationId",
                table: "LocationProducts",
                newName: "IX_LocationProducts_LocationId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationProducts",
                table: "LocationProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationProducts_Locations_LocationId",
                table: "LocationProducts",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationProducts_Products_ProductId",
                table: "LocationProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationProducts_Locations_LocationId",
                table: "LocationProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationProducts_Products_ProductId",
                table: "LocationProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationProducts",
                table: "LocationProducts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "LocationProducts",
                newName: "LocationProduct");

            migrationBuilder.RenameIndex(
                name: "IX_LocationProducts_ProductId",
                table: "LocationProduct",
                newName: "IX_LocationProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationProducts_LocationId",
                table: "LocationProduct",
                newName: "IX_LocationProduct_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationProduct",
                table: "LocationProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationProduct_Locations_LocationId",
                table: "LocationProduct",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationProduct_Products_ProductId",
                table: "LocationProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
