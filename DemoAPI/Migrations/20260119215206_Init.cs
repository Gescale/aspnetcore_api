using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shirts",
                columns: table => new
                {
                    ShirtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shirts", x => x.ShirtId);
                });

            migrationBuilder.InsertData(
                table: "Shirts",
                columns: new[] { "ShirtId", "BrandName", "Colour", "Gender", "Price", "Size" },
                values: new object[,]
                {
                    { 1, "Country Road", "Red", "men", 199, 34 },
                    { 2, "Zara", "Blue", "women", 249, 28 },
                    { 3, "Woolworths", "Green", "men", 149, 30 },
                    { 4, "Polo", "Black", "women", 299, 32 },
                    { 5, "Levis", "White", "men", 176, 36 },
                    { 6, "Guess", "Yellow", "men", 219, 26 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shirts");
        }
    }
}
