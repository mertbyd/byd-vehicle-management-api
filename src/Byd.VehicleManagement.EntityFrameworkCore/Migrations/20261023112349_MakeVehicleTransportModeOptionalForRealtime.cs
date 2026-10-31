using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Byd.VehicleManagement.Migrations
{
    /// <inheritdoc />
    public partial class MakeVehicleTransportModeOptionalForRealtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "transport_mode_id",
                schema: "vehicle_management",
                table: "Vehicles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "transport_mode_id",
                schema: "vehicle_management",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
