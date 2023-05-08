using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tomates.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Celebridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenSrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Celebridad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pelicula",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenSrc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sinopsis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TomatometroCalificacion = table.Column<int>(type: "int", nullable: true),
                    AudienciaCalificacion = table.Column<int>(type: "int", nullable: true),
                    TomatometroContadorCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudienciaContadorCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clasificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LenguajeOriginal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaLanzamientoCine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaLanzamientoTransmision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duracion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distribuidor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoProduccion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pelicula", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plataforma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenSrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plataforma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Serie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenSrc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sinopsis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TomatometroCalificacion = table.Column<int>(type: "int", nullable: true),
                    AudienciaCalificacion = table.Column<int>(type: "int", nullable: true),
                    TomatometroContadorCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudienciaContadorCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechalanzamientoTransmision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temporada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerieId = table.Column<int>(type: "int", nullable: false),
                    Numtemporada = table.Column<int>(type: "int", nullable: false),
                    TomatometroCalificacion = table.Column<int>(type: "int", nullable: true),
                    AudienciaCalificacion = table.Column<int>(type: "int", nullable: true),
                    TomatometroContadorCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudienciaContadorCalificacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temporada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temporada_Serie_SerieId",
                        column: x => x.SerieId,
                        principalTable: "Serie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Temporada_SerieId",
                table: "Temporada",
                column: "SerieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Celebridad");

            migrationBuilder.DropTable(
                name: "Pelicula");

            migrationBuilder.DropTable(
                name: "Plataforma");

            migrationBuilder.DropTable(
                name: "Temporada");

            migrationBuilder.DropTable(
                name: "Serie");
        }
    }
}
