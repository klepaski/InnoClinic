using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "Role" },
                values: new object[,]
                {
                    { 1, "user@gmail.com", "by[.�#[;M(�i��6g���W��ـ�e37", "pXX3ZevB7sJMmgj4EkNH0+0YNYxZgUtNrZWkyhvQUOY=", new DateTime(2023, 9, 20, 13, 40, 28, 725, DateTimeKind.Unspecified).AddTicks(6178), 0 },
                    { 2, "doctor@gmail.com", "by[.�#[;M(�i��6g���W��ـ�e37", "pXX3ZevB7sJMmgj4EkNH0+0YNYxZgUtNrZWkyhvQUOY=", new DateTime(2023, 9, 20, 13, 40, 28, 725, DateTimeKind.Unspecified).AddTicks(6178), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
