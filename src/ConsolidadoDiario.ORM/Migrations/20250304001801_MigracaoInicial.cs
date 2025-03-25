using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsolidadoDiario.ORM.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidadoDiarioContas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroConta = table.Column<string>(type: "text", nullable: false),
                    NumeroAgencia = table.Column<string>(type: "text", nullable: false),
                    DataConsolidacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalCreditos = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalDebitos = table.Column<decimal>(type: "numeric", nullable: false),
                    SaldoConsolidado = table.Column<decimal>(type: "numeric", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsolidadoDiarioContas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidadoDiarioContas");
        }
    }
}
