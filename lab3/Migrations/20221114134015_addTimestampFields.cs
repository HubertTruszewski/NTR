using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lab2.Migrations
{
    public partial class addTimestampFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "rowVersion",
                table: "users",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "rowVersion",
                table: "books",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rowVersion",
                table: "users");

            migrationBuilder.DropColumn(
                name: "rowVersion",
                table: "books");
        }
    }
}
