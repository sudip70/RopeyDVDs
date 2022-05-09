using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RopeyDVD.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MembershipCategories_MembershipCategoryNumber",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipCategoryNumber",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MembershipCategories_MembershipCategoryNumber",
                table: "Members",
                column: "MembershipCategoryNumber",
                principalTable: "MembershipCategories",
                principalColumn: "MembershipCategoryNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MembershipCategories_MembershipCategoryNumber",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipCategoryNumber",
                table: "Members",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MembershipCategories_MembershipCategoryNumber",
                table: "Members",
                column: "MembershipCategoryNumber",
                principalTable: "MembershipCategories",
                principalColumn: "MembershipCategoryNumber");
        }
    }
}
