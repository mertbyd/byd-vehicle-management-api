using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: Vehicle tablosunun adini, tekillik indekslerini, alan uzunluklarini ve realtime degerlerinin lookup kodu
// karsiliklarini tanimlar.
// sistemdeki gorevi: Entity, EF eslemesi, validator ve realtime kaydinin ayni arac sozlesmesini okumasini saglar.
public static class VehicleConsts
{
    public const string TableName = "Vehicles";
    public const string GtfsVehicleIdIndex = "IX_Vehicles_Scope_Agency_GtfsVehicleId";
    public const string RegistrationNumberIndex = "IX_Vehicles_Tenant_RegistrationNumber";

    // Plakasi girilmemis araclar tekillige katilmaz; filtre veritabani kolon adiyla yazilir.
    public const string RegistrationNumberIndexFilter = "registration_number IS NOT NULL";

    public const int MaxGtfsVehicleIdLength = 100;
    public const int MaxLabelLength = 50;
    public const int MaxRegistrationNumberLength = 20;
    public const int MaxGtfsAgencyIdLength = 100;

    // Realtime eslesmesinin okudugu organizasyon arac listesinin cache omru ve anahtar ayiricisi.
    public const int CacheDurationInSeconds = 3600;
    public const string CacheKeyOrganizationSeparator = ":";

    // GTFS genisletilmis route_type araliklarinin temel route_type karsiligi; temel deger oldugu gibi kullanilir. Aralik
    // icindeki ozel deger araligin genelinden once yazilir.
    public static readonly IReadOnlyList<(int From, int To, int RouteType)> ExtendedRouteTypes =
    [
        (100, 199, 2),
        (200, 299, 3),
        (405, 405, 12),
        (400, 499, 1),
        (700, 799, 3),
        (800, 899, 11),
        (900, 999, 0),
        (1000, 1099, 4),
        (1200, 1299, 4),
        (1300, 1399, 6),
        (1400, 1499, 7)
    ];

    // GTFS-RT VehicleDescriptor.wheelchair_accessible degerinin WheelchairAccessible lookup koduna karsiligi; NO_VALUE (0)
    // listede yoktur, bilgi bos kalir.
    public static readonly IReadOnlyList<(int RealtimeValue, int Code)> RealtimeWheelchairAccessibleCodes =
    [
        (1, 0),
        (2, 1),
        (3, 2)
    ];
}
