using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeKeeper.Migrations
{
    public partial class AddColumToEmploy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasImg",
                table: "Empleados",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasImg",
                table: "Empleados");
        }
    }
}
