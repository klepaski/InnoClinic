using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OfficesAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistryPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "City", "HouseNumber", "OfficeNumber", "PhotoId", "PhotoUrl", "RegistryPhoneNumber", "Status", "Street" },
                values: new object[,]
                {
                    { 1, "Brest", "4", "54", null, "https://avatars.mds.yandex.net/get-altay/4464784/2a0000017840924485aa42a8ef3d614ace76/L_height", "+375333186223", 1, "Kolesnika" },
                    { 2, "Minsk", "13a", "226", null, "https://avatars.mds.yandex.net/get-altay/933207/2a00000161bf61714998e315745eea065577/orig", "+375333733839", 0, "Sverdlova" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
