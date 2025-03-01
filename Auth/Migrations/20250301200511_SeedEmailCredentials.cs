using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmailCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Emails] ([UserName],[Password],[SmtpServer],[Port],[SenderName])" +
                "VALUES (N'marooo7878@outlook.com', N'sAQoShCD58JZ5fWg+P+VTM8hU3LudflcG9izRdpIPqM=', N'smtp.office365.com',587, N'Marwan Mohamed')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Emails]");
        }
    }
}
