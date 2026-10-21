namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: EquipmentType lookup satirini tasir.
// sistemdeki gorevi: Arac kayitlarinin donanim turu degerine kodla baglanmasini saglar.
public class EquipmentType : VehicleLookup
{
    // EF materialization icin bos kabuk kurar.
    protected EquipmentType()
    {
    }

    // Kimligi, adi ve kodu verilen donanim turu satirini kurar.
    internal EquipmentType(int id, string name, int code) : base(id, name, code)
    {
    }
}
