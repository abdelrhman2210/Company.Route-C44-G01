using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Route_C44_G01.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageNameColoumnToEmployeeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Employees");
        }
    }
}
