using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Byd.VehicleManagement.Migrations
{
    /// <inheritdoc />
    public partial class CreateVehicleModelsAndVehicleTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleModels",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    manufacturer = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vehicle_model_id = table.Column<Guid>(type: "uuid", nullable: true),
                    transport_mode_id = table.Column<int>(type: "integer", nullable: false),
                    fuel_type_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    seating_capacity = table.Column<int>(type: "integer", nullable: false),
                    standing_capacity = table.Column<int>(type: "integer", nullable: false),
                    wheelchair_place_capacity = table.Column<int>(type: "integer", nullable: false),
                    bicycle_capacity = table.Column<int>(type: "integer", nullable: false),
                    low_floor = table.Column<bool>(type: "boolean", nullable: false),
                    has_lift_or_ramp = table.Column<bool>(type: "boolean", nullable: false),
                    length = table.Column<decimal>(type: "numeric", nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_types_fuel_types_fuel_type_id",
                        column: x => x.fuel_type_id,
                        principalSchema: "vehicle_management",
                        principalTable: "FuelTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicle_types_transport_modes_transport_mode_id",
                        column: x => x.transport_mode_id,
                        principalSchema: "vehicle_management",
                        principalTable: "TransportModes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicle_types_vehicle_models_vehicle_model_id",
                        column: x => x.vehicle_model_id,
                        principalSchema: "vehicle_management",
                        principalTable: "VehicleModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_Tenant_Manufacturer_Name",
                schema: "vehicle_management",
                table: "VehicleModels",
                columns: new[] { "tenant_id", "manufacturer", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_Scope_Code",
                schema: "vehicle_management",
                table: "VehicleTypes",
                columns: new[] { "tenant_id", "organization_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_fuel_type_id",
                schema: "vehicle_management",
                table: "VehicleTypes",
                column: "fuel_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_transport_mode_id",
                schema: "vehicle_management",
                table: "VehicleTypes",
                column: "transport_mode_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_types_vehicle_model_id",
                schema: "vehicle_management",
                table: "VehicleTypes",
                column: "vehicle_model_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleTypes",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "VehicleModels",
                schema: "vehicle_management");
        }
    }
}
