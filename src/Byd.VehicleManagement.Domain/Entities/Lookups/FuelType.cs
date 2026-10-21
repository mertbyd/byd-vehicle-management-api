namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: FuelType lookup satirini tasir.
// sistemdeki gorevi: Arac kayitlarinin yakit turu degerine kodla baglanmasini saglar.
public class FuelType : VehicleLookup
{
    // EF materialization icin bos kabuk kurar.
    protected FuelType()
    {
    }

    // Kimligi, adi ve kodu verilen yakit turu satirini kurar.
    internal FuelType(int id, string name, int code) : base(id, name, code)
    {
    }
}
