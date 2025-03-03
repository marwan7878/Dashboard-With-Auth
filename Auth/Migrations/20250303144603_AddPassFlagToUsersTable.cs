using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPassFlagToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordChanged",
                schema: "Security",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
            migrationBuilder.Sql("UPDATE [Security].[Users] SET [IsPasswordChanged] = 1 WHERE Id = N'2b723733-db64-4941-ad51-b9a6cd6a7ed4'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordChanged",
                schema: "Security",
                table: "Users");
        }
    }
}


