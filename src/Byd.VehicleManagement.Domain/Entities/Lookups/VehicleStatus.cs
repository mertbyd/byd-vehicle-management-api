namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: VehicleStatus lookup satirini tasir.
// sistemdeki gorevi: Arac kayitlarinin arac durumu degerine kodla baglanmasini saglar.
public class VehicleStatus : VehicleLookup
{
    // EF materialization icin bos kabuk kurar.
    protected VehicleStatus()
    {
    }

    // Kimligi, adi ve kodu verilen arac durumu satirini kurar.
    internal VehicleStatus(int id, string name, int code) : base(id, name, code)
    {
    }
}
