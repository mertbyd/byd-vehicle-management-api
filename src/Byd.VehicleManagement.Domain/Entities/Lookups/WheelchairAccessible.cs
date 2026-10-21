namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: WheelchairAccessible lookup satirini tasir.
// sistemdeki gorevi: Arac kayitlarinin tekerlekli sandalye erisimi degerine kodla baglanmasini saglar.
public class WheelchairAccessible : VehicleLookup
{
    // EF materialization icin bos kabuk kurar.
    protected WheelchairAccessible()
    {
    }

    // Kimligi, adi ve kodu verilen tekerlekli sandalye erisimi satirini kurar.
    internal WheelchairAccessible(int id, string name, int code) : base(id, name, code)
    {
    }
}
