using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeKeeper.Migrations
{
    public partial class SetTimeOutOfRangeAndHasThisTimeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasTimeOutOfRange",
                table: "Tiempos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeOut",
                table: "Tiempos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTimeOutOfRange",
                table: "Tiempos");

            migrationBuilder.DropColumn(
                name: "TimeOut",
                table: "Tiempos");
        }
    }
}
