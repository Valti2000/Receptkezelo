using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recept.Migrations
{
    public partial class Proba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Csoport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Csoport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receptek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leiras = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    ElokeszitesiIdo = table.Column<int>(type: "int", nullable: false),
                    FozesiIdo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptek", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alapanyag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KategoriaId = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alapanyag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alapanyag_Kategoria_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "Kategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlapanyagAllergen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tartalmaz = table.Column<bool>(type: "bit", nullable: false),
                    AlapanyagId = table.Column<int>(type: "int", nullable: false),
                    AllergenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlapanyagAllergen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlapanyagAllergen_Alapanyag_AlapanyagId",
                        column: x => x.AlapanyagId,
                        principalTable: "Alapanyag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlapanyagAllergen_Allergen_AllergenId",
                        column: x => x.AllergenId,
                        principalTable: "Allergen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hozzavalo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlapanyagId = table.Column<int>(type: "int", nullable: false),
                    Mennyiseg = table.Column<double>(type: "float", nullable: false),
                    Mertekegyseg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CsoportId = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hozzavalo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hozzavalo_Alapanyag_AlapanyagId",
                        column: x => x.AlapanyagId,
                        principalTable: "Alapanyag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hozzavalo_Csoport_CsoportId",
                        column: x => x.CsoportId,
                        principalTable: "Csoport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceptHozzavalo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HozzavaloId = table.Column<int>(type: "int", nullable: false),
                    ReceptId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptHozzavalo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptHozzavalo_Hozzavalo_HozzavaloId",
                        column: x => x.HozzavaloId,
                        principalTable: "Hozzavalo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceptHozzavalo_Receptek_ReceptId",
                        column: x => x.ReceptId,
                        principalTable: "Receptek",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alapanyag_KategoriaId",
                table: "Alapanyag",
                column: "KategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlapanyagAllergen_AlapanyagId",
                table: "AlapanyagAllergen",
                column: "AlapanyagId");

            migrationBuilder.CreateIndex(
                name: "IX_AlapanyagAllergen_AllergenId",
                table: "AlapanyagAllergen",
                column: "AllergenId");

            migrationBuilder.CreateIndex(
                name: "IX_Hozzavalo_AlapanyagId",
                table: "Hozzavalo",
                column: "AlapanyagId");

            migrationBuilder.CreateIndex(
                name: "IX_Hozzavalo_CsoportId",
                table: "Hozzavalo",
                column: "CsoportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptHozzavalo_HozzavaloId",
                table: "ReceptHozzavalo",
                column: "HozzavaloId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptHozzavalo_ReceptId",
                table: "ReceptHozzavalo",
                column: "ReceptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlapanyagAllergen");

            migrationBuilder.DropTable(
                name: "ReceptHozzavalo");

            migrationBuilder.DropTable(
                name: "Allergen");

            migrationBuilder.DropTable(
                name: "Hozzavalo");

            migrationBuilder.DropTable(
                name: "Receptek");

            migrationBuilder.DropTable(
                name: "Alapanyag");

            migrationBuilder.DropTable(
                name: "Csoport");

            migrationBuilder.DropTable(
                name: "Kategoria");
        }
    }
}
