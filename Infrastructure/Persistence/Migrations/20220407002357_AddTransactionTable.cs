using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryType",
                table: "Category",
                newName: "CategoryTypeLookupId");

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

            migrationBuilder.CreateTable(
                name: "TransactionTypeLookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeLookup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionTypeLookupId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UserUniqueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UniqueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_TransactionTypeLookup_TransactionTypeLookupId",
                        column: x => x.TransactionTypeLookupId,
                        principalTable: "TransactionTypeLookup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryTypeLookupId",
                table: "Category",
                column: "CategoryTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CategoryId",
                table: "Transaction",
                column: "CategoryId");

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

            migrationBuilder.InsertData(
                table: "CategoryTypeLookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Expenses" });

            migrationBuilder.InsertData(
                table: "CategoryTypeLookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Income" });

            migrationBuilder.InsertData(
                table: "TransactionTypeLookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Expenses" });

            migrationBuilder.InsertData(
                table: "TransactionTypeLookup",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Income" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_CategoryTypeLookup_CategoryTypeLookupId",
                table: "Category");

            migrationBuilder.DropTable(
                name: "CategoryTypeLookup");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "TransactionTypeLookup");

            migrationBuilder.DropIndex(
                name: "IX_Category_CategoryTypeLookupId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoryTypeLookupId",
                table: "Category",
                newName: "CategoryType");
        }
    }
}
