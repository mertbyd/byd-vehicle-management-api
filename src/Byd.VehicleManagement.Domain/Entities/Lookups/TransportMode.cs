namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: TransportMode lookup satirini tasir.
// sistemdeki gorevi: Arac kayitlarinin ulasim modu degerine kodla baglanmasini saglar.
public class TransportMode : VehicleLookup
{
    // EF materialization icin bos kabuk kurar.
    protected TransportMode()
    {
    }

    // Kimligi, adi ve kodu verilen ulasim modu satirini kurar.
    internal TransportMode(int id, string name, int code) : base(id, name, code)
    {
    }
}
