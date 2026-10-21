namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: RegistrationSource lookup satirini tasir.
// sistemdeki gorevi: Arac kayitlarinin kayit kaynagi degerine kodla baglanmasini saglar.
public class RegistrationSource : VehicleLookup
{
    // EF materialization icin bos kabuk kurar.
    protected RegistrationSource()
    {
    }

    // Kimligi, adi ve kodu verilen kayit kaynagi satirini kurar.
    internal RegistrationSource(int id, string name, int code) : base(id, name, code)
    {
    }
}
