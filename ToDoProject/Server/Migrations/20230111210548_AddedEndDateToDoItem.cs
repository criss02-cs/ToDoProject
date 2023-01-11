using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoProject.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedEndDateToDoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ToDoItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ToDoItems");
        }
    }
}
