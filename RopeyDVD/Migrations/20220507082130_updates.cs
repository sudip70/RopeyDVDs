using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RopeyDVD.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberBirthOfDate",
                table: "Members",
                newName: "MemberDOB");

            migrationBuilder.AlterColumn<string>(
                name: "MemberFirstName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberDOB",
                table: "Members",
                newName: "MemberBirthOfDate");

            migrationBuilder.AlterColumn<int>(
                name: "MemberFirstName",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
