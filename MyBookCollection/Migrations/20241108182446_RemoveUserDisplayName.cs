﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookCollection.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserDisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "DisplayName",
               table: "AspNetUsers",
               type: "nvarchar(max)",
               nullable: false,
               defaultValue: "");
        }
    }
}
