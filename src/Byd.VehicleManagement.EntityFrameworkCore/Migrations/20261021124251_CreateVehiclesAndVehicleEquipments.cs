using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Byd.VehicleManagement.Migrations
{
    /// <inheritdoc />
    public partial class CreateVehiclesAndVehicleEquipments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationSources",
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
                    table.PrimaryKey("pk_registration_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vehicle_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    transport_mode_id = table.Column<int>(type: "integer", nullable: false),
                    vehicle_status_id = table.Column<int>(type: "integer", nullable: false),
                    registration_source_id = table.Column<int>(type: "integer", nullable: false),
                    wheelchair_accessible_id = table.Column<int>(type: "integer", nullable: true),
                    gtfs_vehicle_id = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: true),
                    registration_number = table.Column<string>(type: "text", nullable: true),
                    gtfs_agency_id = table.Column<string>(type: "text", nullable: true),
                    in_service_from = table.Column<DateOnly>(type: "date", nullable: false),
                    in_service_until = table.Column<DateOnly>(type: "date", nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_registration_sources_registration_source_id",
                        column: x => x.registration_source_id,
                        principalSchema: "vehicle_management",
                        principalTable: "RegistrationSources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicles_transport_modes_transport_mode_id",
                        column: x => x.transport_mode_id,
                        principalSchema: "vehicle_management",
                        principalTable: "TransportModes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicles_vehicle_statuses_vehicle_status_id",
                        column: x => x.vehicle_status_id,
                        principalSchema: "vehicle_management",
                        principalTable: "VehicleStatuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicles_vehicle_types_vehicle_type_id",
                        column: x => x.vehicle_type_id,
                        principalSchema: "vehicle_management",
                        principalTable: "VehicleTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicles_wheelchair_accessibles_wheelchair_accessible_id",
                        column: x => x.wheelchair_accessible_id,
                        principalSchema: "vehicle_management",
                        principalTable: "WheelchairAccessibles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleEquipments",
                schema: "vehicle_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_type_id = table.Column<int>(type: "integer", nullable: false),
                    units = table.Column<int>(type: "integer", nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_equipments", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicle_equipments_equipment_types_equipment_type_id",
                        column: x => x.equipment_type_id,
                        principalSchema: "vehicle_management",
                        principalTable: "EquipmentTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehicle_equipments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "vehicle_management",
                        principalTable: "Vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSources_Code",
                schema: "vehicle_management",
                table: "RegistrationSources",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleEquipments_Vehicle_EquipmentType",
                schema: "vehicle_management",
                table: "VehicleEquipments",
                columns: new[] { "vehicle_id", "equipment_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_equipments_equipment_type_id",
                schema: "vehicle_management",
                table: "VehicleEquipments",
                column: "equipment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Scope_Agency_GtfsVehicleId",
                schema: "vehicle_management",
                table: "Vehicles",
                columns: new[] { "tenant_id", "organization_id", "gtfs_agency_id", "gtfs_vehicle_id" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Tenant_RegistrationNumber",
                schema: "vehicle_management",
                table: "Vehicles",
                columns: new[] { "tenant_id", "registration_number" },
                unique: true,
                filter: "registration_number IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_registration_source_id",
                schema: "vehicle_management",
                table: "Vehicles",
                column: "registration_source_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_transport_mode_id",
                schema: "vehicle_management",
                table: "Vehicles",
                column: "transport_mode_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_vehicle_status_id",
                schema: "vehicle_management",
                table: "Vehicles",
                column: "vehicle_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_vehicle_type_id",
                schema: "vehicle_management",
                table: "Vehicles",
                column: "vehicle_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_wheelchair_accessible_id",
                schema: "vehicle_management",
                table: "Vehicles",
                column: "wheelchair_accessible_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleEquipments",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "vehicle_management");

            migrationBuilder.DropTable(
                name: "RegistrationSources",
                schema: "vehicle_management");
        }
    }
}
