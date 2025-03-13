using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Contacts");
        }
    }
}
