using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restoran.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gost",
                columns: table => new
                {
                    IDGosta = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImeGosta = table.Column<string>(type: "TEXT", nullable: false),
                    PrezimeGosta = table.Column<string>(type: "TEXT", nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gost", x => x.IDGosta);
                });

            migrationBuilder.CreateTable(
                name: "Sto",
                columns: table => new
                {
                    IDStola = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrojStola = table.Column<int>(type: "INTEGER", nullable: false),
                    BrojMesta = table.Column<int>(type: "INTEGER", nullable: false),
                    Lokacija = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sto", x => x.IDStola);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    IDRezervacije = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Vreme = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    BrojOsoba = table.Column<int>(type: "INTEGER", nullable: false),
                    IDGosta = table.Column<int>(type: "INTEGER", nullable: false),
                    IDStola = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.IDRezervacije);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Gost_IDGosta",
                        column: x => x.IDGosta,
                        principalTable: "Gost",
                        principalColumn: "IDGosta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Sto_IDStola",
                        column: x => x.IDStola,
                        principalTable: "Sto",
                        principalColumn: "IDStola",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_IDGosta",
                table: "Rezervacija",
                column: "IDGosta");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_IDStola",
                table: "Rezervacija",
                column: "IDStola");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "Gost");

            migrationBuilder.DropTable(
                name: "Sto");
        }
    }
}
