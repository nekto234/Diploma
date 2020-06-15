using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPlatform.Migrations
{
    public partial class AddNameColumnToCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HasTest",
                table: "Module",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasLab",
                table: "Module",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Course",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Course");

            migrationBuilder.AlterColumn<bool>(
                name: "HasTest",
                table: "Module",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "HasLab",
                table: "Module",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
