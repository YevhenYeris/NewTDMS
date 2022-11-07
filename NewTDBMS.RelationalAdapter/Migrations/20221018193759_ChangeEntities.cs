using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewTDBMS.RelationalAdapter.Migrations
{
    public partial class ChangeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBs",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBs", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DBName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => new { x.DBName, x.Name });
                    table.ForeignKey(
                        name: "FK_Tables_DBs_DBName",
                        column: x => x.DBName,
                        principalTable: "DBs",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DBName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableModelDBName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TableModelName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Columns_Tables_DBName_TableName",
                        columns: x => new { x.DBName, x.TableName },
                        principalTable: "Tables",
                        principalColumns: new[] { "DBName", "Name" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Columns_Tables_TableModelDBName_TableModelName",
                        columns: x => new { x.TableModelDBName, x.TableModelName },
                        principalTable: "Tables",
                        principalColumns: new[] { "DBName", "Name" });
                });

            migrationBuilder.CreateTable(
                name: "Rows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DBName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableModelDBName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TableModelName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rows_Tables_DBName_TableName",
                        columns: x => new { x.DBName, x.TableName },
                        principalTable: "Tables",
                        principalColumns: new[] { "DBName", "Name" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rows_Tables_TableModelDBName_TableModelName",
                        columns: x => new { x.TableModelDBName, x.TableModelName },
                        principalTable: "Tables",
                        principalColumns: new[] { "DBName", "Name" });
                });

            migrationBuilder.CreateTable(
                name: "RowValues",
                columns: table => new
                {
                    DBName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowValues", x => new { x.DBName, x.TableName, x.RowId, x.Value });
                    table.ForeignKey(
                        name: "FK_RowValues_Rows_RowId",
                        column: x => x.RowId,
                        principalTable: "Rows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Columns_DBName_TableName",
                table: "Columns",
                columns: new[] { "DBName", "TableName" });

            migrationBuilder.CreateIndex(
                name: "IX_Columns_TableModelDBName_TableModelName",
                table: "Columns",
                columns: new[] { "TableModelDBName", "TableModelName" });

            migrationBuilder.CreateIndex(
                name: "IX_Rows_DBName_TableName",
                table: "Rows",
                columns: new[] { "DBName", "TableName" });

            migrationBuilder.CreateIndex(
                name: "IX_Rows_TableModelDBName_TableModelName",
                table: "Rows",
                columns: new[] { "TableModelDBName", "TableModelName" });

            migrationBuilder.CreateIndex(
                name: "IX_RowValues_RowId",
                table: "RowValues",
                column: "RowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "RowValues");

            migrationBuilder.DropTable(
                name: "Rows");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "DBs");
        }
    }
}
