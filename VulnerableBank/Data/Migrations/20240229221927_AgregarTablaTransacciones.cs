using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VulnerableBank.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaTransacciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceAccountNumber = table.Column<int>(type: "int", nullable: false),
                    DestinationAccountNumber = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_DestinationAccountNumber",
                        column: x => x.DestinationAccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_SourceAccountNumber",
                        column: x => x.SourceAccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DestinationAccountNumber",
                table: "Transactions",
                column: "DestinationAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceAccountNumber",
                table: "Transactions",
                column: "SourceAccountNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
