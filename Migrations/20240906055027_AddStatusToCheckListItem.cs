using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckListAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToCheckListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CheckListItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CheckListItems");
        }
    }
}
