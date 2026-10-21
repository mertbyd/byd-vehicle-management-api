using System;
using Byd.CoreExtensions.Interfaces;
using Volo.Abp.Domain.Entities;

namespace Byd.VehicleManagement.Entities.Lookups;

// islevi: Host kapsamindaki arac lookup satirlarinin ortak Id, Code ve Name alanlarini tasir.
// sistemdeki gorevi: Her lookup turunun ayni alanlari ve ILookup sozlesmesini tekrar yazmasini onler.
public abstract class VehicleLookup : Entity<int>, ILookup
{
    // Lookup turunde tekil olan ve dis sistemlerle paylasilan kod.
    public int Code { get; internal set; }

    // Kaydin kullaniciya gorunen adi.
    public string Name { get; internal set; } = default!;

    // EF materialization icin bos kabuk kurar.
    protected VehicleLookup()
    {
    }

    // Kimligi, adi ve kodu verilen lookup satirini kurar.
    protected VehicleLookup(int id, string name, int code) : base(id)
    {
        Code = code;
        Name = name;
    }

    // ILookup ad yazma sozlesmesini public entity setter acmadan uygular.
    string ILookup.Name
    {
        get => Name;
        set => Name = value;
    }

    // ILookup kod yazma sozlesmesini public entity setter acmadan uygular.
    int? ILookup.Code
    {
        get => Code;
        set => Code = value ?? throw new ArgumentNullException(nameof(value));
    }
}
