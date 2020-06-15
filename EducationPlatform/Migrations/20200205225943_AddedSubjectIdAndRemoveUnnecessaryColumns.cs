using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPlatform.Migrations
{
    public partial class AddedSubjectIdAndRemoveUnnecessaryColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Course_CreatedBy_AspNetUsers",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_CreatedBy",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Course");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CourseModule",
                type: "date",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Course",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Course_SubjectId",
                table: "Course",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_SubjectId",
                table: "Course",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_SubjectId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_SubjectId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "CourseModule");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Course");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Course",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Course",
                type: "date",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Course",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_CreatedBy",
                table: "Course",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "fk_Course_CreatedBy_AspNetUsers",
                table: "Course",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
