using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Byd.VehicleManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialVehicleLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "vehicle_management");

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fuel_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TransportModes",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transport_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleStatuses",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WheelchairAccessibles",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wheelchair_accessibles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypes_Code",
                schema: "vehicle_management",
                table: "EquipmentTypes",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelTypes_Code",
                schema: "vehicle_management",
                table: "FuelTypes",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportModes_Code",
                schema: "vehicle_management",
                table: "TransportModes",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStatuses_Code",
                schema: "vehicle_management",
                table: "VehicleStatuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WheelchairAccessibles_Code",
                schema: "vehicle_management",
                table: "WheelchairAccessibles",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentTypes",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "FuelTypes",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "TransportModes",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "VehicleStatuses",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "WheelchairAccessibles",
                schema: "vehicle_management");
        }
    }
}
