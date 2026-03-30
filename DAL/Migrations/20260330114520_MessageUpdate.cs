using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MessageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageSent",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "MessagesSentToday",
                table: "Users",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageSent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MessagesSentToday",
                table: "Users");
        }
    }
}
