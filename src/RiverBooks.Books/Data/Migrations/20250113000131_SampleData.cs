using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiverBooks.Books.Data.Migrations
{
    /// <inheritdoc />
    public partial class SampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Books",
                table: "Books",
                columns: new[] { "Id", "Author", "Price", "Title" },
                values: new object[,]
                {
                    { new Guid("17c61e41-3953-42cd-8f88-d3f698869b35"), "J.R.R. Tolkien", 12.99m, "The Return of the King" },
                    { new Guid("a89f6cd7-4693-457b-9009-02205dbbfe45"), "J.R.R. Tolkien", 10.99m, "The Fellowship of the Ring" },
                    { new Guid("e4fa19bf-6981-4e50-a542-7c9b26e9ec31"), "J.R.R. Tolkien", 11.99m, "The Two Towers" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Books",
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("17c61e41-3953-42cd-8f88-d3f698869b35"));

            migrationBuilder.DeleteData(
                schema: "Books",
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("a89f6cd7-4693-457b-9009-02205dbbfe45"));

            migrationBuilder.DeleteData(
                schema: "Books",
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e4fa19bf-6981-4e50-a542-7c9b26e9ec31"));
        }
    }
}
