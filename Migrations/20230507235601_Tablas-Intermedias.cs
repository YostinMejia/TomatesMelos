using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tomates.Migrations
{
    public partial class TablasIntermedias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagenSrc",
                table: "Plataforma",
                newName: "Link");

            migrationBuilder.CreateTable(
                name: "PeliculaCelebridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    CelebridadId = table.Column<int>(type: "int", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaCelebridad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeliculaCelebridad_Celebridad_CelebridadId",
                        column: x => x.CelebridadId,
                        principalTable: "Celebridad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaCelebridad_Pelicula_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Pelicula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculaPlataforma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    PlataformaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaPlataforma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeliculaPlataforma_Pelicula_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Pelicula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaPlataforma_Plataforma_PlataformaId",
                        column: x => x.PlataformaId,
                        principalTable: "Plataforma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriePlataforma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerieId = table.Column<int>(type: "int", nullable: false),
                    PlataformaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriePlataforma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriePlataforma_Plataforma_PlataformaId",
                        column: x => x.PlataformaId,
                        principalTable: "Plataforma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriePlataforma_Serie_SerieId",
                        column: x => x.SerieId,
                        principalTable: "Serie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemporadaCelebridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemporadaId = table.Column<int>(type: "int", nullable: false),
                    CelebridadId = table.Column<int>(type: "int", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporadaCelebridad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporadaCelebridad_Celebridad_CelebridadId",
                        column: x => x.CelebridadId,
                        principalTable: "Celebridad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporadaCelebridad_Temporada_TemporadaId",
                        column: x => x.TemporadaId,
                        principalTable: "Temporada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaCelebridad_CelebridadId",
                table: "PeliculaCelebridad",
                column: "CelebridadId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaCelebridad_PeliculaId",
                table: "PeliculaCelebridad",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaPlataforma_PeliculaId",
                table: "PeliculaPlataforma",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaPlataforma_PlataformaId",
                table: "PeliculaPlataforma",
                column: "PlataformaId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriePlataforma_PlataformaId",
                table: "SeriePlataforma",
                column: "PlataformaId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriePlataforma_SerieId",
                table: "SeriePlataforma",
                column: "SerieId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporadaCelebridad_CelebridadId",
                table: "TemporadaCelebridad",
                column: "CelebridadId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporadaCelebridad_TemporadaId",
                table: "TemporadaCelebridad",
                column: "TemporadaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeliculaCelebridad");

            migrationBuilder.DropTable(
                name: "PeliculaPlataforma");

            migrationBuilder.DropTable(
                name: "SeriePlataforma");

            migrationBuilder.DropTable(
                name: "TemporadaCelebridad");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Plataforma",
                newName: "ImagenSrc");
        }
    }
}
