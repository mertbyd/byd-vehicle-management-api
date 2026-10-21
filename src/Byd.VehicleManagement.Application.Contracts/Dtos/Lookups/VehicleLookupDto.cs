namespace Byd.VehicleManagement.Dtos.Lookups;

// islevi: Arac lookup satirinin kimligini, kodunu ve adini tasir.
// sistemdeki gorevi: Persistence entity'sini public cevap sozlesmesinden ayirir.
public class VehicleLookupDto
{
    // Primary Key
    public int Id { get; set; }

    // Table Columns / Data
    public int Code { get; set; }
    public string Name { get; set; } = default!;
}
