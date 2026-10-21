using System;

namespace Byd.VehicleManagement.Dtos.VehicleEquipments;

// islevi: Arac donaniminin aracini, turunu, adedini ve bagli kayitlarin gorunen degerlerini tasir.
// sistemdeki gorevi: Persistence entity'sini public cevap sozlesmesinden ayirir; istemci aracin GTFS-RT kimligini ve
// donanim turu adini ikinci bir istek atmadan gosterir.
public class VehicleEquipmentDto
{
    // Primary Key
    public Guid Id { get; set; }

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public int EquipmentTypeId { get; set; }

    // Table Columns / Data
    public string VehicleGtfsVehicleId { get; set; } = default!;
    public string EquipmentTypeName { get; set; } = default!;
    public int Units { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}
