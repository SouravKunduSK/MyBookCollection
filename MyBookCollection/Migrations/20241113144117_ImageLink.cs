﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookCollection.Migrations
{
    /// <inheritdoc />
    public partial class ImageLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "AspNetUsers");
        }
    }
}
