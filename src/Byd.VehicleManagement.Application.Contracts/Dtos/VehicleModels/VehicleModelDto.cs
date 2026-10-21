using System;

namespace Byd.VehicleManagement.Dtos.VehicleModels;

// islevi: Arac modelinin kimligini, uretici ve model adini ve audit zamanlarini tasir.
// sistemdeki gorevi: Persistence entity'sini public cevap sozlesmesinden ayirir.
public class VehicleModelDto
{
    // Primary Key
    public Guid Id { get; set; }

    // Table Columns / Data
    public string Manufacturer { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}
