using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen2Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Document = table.Column<string>(type: "TEXT", nullable: false),
                    HireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    SalaryBase = table.Column<decimal>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planillas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Period = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planillas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPlanilla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PayrollId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaryBase = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExtraHours = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExtraHoursAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Bonuses = table.Column<decimal>(type: "TEXT", nullable: false),
                    Deductions = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetSalary = table.Column<decimal>(type: "TEXT", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPlanilla", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPlanilla_Empleados_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPlanilla_Planillas_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Planillas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPlanilla_EmployeeId",
                table: "DetallesPlanilla",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPlanilla_PayrollId",
                table: "DetallesPlanilla",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Document",
                table: "Empleados",
                column: "Document",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesPlanilla");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Planillas");
        }
    }
}
