using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: RegistrationSource tablosunun kalici kimliklerini ve kodlarini tanimlar.
// sistemdeki gorevi: Seed akisi, EF eslemesi ve aracin kaynagini kodla bulan Manager'in ayni satirlari okumasini saglar.
public static class RegistrationSourceConsts
{
    public const string TableName = "RegistrationSources";
    public const string CodeIndex = "IX_RegistrationSources_Code";

    // Realtime akisinda gorulup otomatik eklenen arac.
    public const int RealtimeCode = 1;

    // Yonetim ekranindan elle eklenen arac.
    public const int ManualCode = 2;

    public static readonly IReadOnlyList<(int Id, string Name, int Code)> Definitions =
    [
        (1, "Realtime", RealtimeCode),
        (2, "Manuel", ManualCode)
    ];
}
