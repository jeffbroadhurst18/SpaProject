using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SpaProject.Data.Migrations
{
    public partial class Sun0104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "County",
                table: "Address",
                newName: "Country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Address",
                newName: "County");
        }
    }
}
