using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HashCraft.Storage.dal.migrations
{
    /// <inheritdoc />
    public partial class Rename_HashStat_Count : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "HashStats",
                newName: "Count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "HashStats",
                newName: "Quantity");
        }
    }
}
