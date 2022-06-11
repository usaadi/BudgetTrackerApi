using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class RemovedCategoryTypeLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_CategoryTypeLookup_CategoryTypeLookupId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionTypeLookup_TransactionTypeLookupId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "CategoryTypeLookup");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TransactionTypeLookupId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionTypeLookupId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "CategoryTypeLookupId",
                table: "Category",
                newName: "TransactionTypeLookupId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_CategoryTypeLookupId",
                table: "Category",
                newName: "IX_Category_TransactionTypeLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_TransactionTypeLookup_TransactionTypeLookupId",
                table: "Category",
                column: "TransactionTypeLookupId",
                principalTable: "TransactionTypeLookup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_TransactionTypeLookup_TransactionTypeLookupId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeLookupId",
                table: "Category",
                newName: "CategoryTypeLookupId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_TransactionTypeLookupId",
                table: "Category",
                newName: "IX_Category_CategoryTypeLookupId");

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeLookupId",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryTypeLookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypeLookup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionTypeLookupId",
                table: "Transaction",
                column: "TransactionTypeLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_CategoryTypeLookup_CategoryTypeLookupId",
                table: "Category",
                column: "CategoryTypeLookupId",
                principalTable: "CategoryTypeLookup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionTypeLookup_TransactionTypeLookupId",
                table: "Transaction",
                column: "TransactionTypeLookupId",
                principalTable: "TransactionTypeLookup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
