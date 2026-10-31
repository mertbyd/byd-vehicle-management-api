using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: VehicleStatus tablosunun kalici kimliklerini ve kodlarini tanimlar.
// sistemdeki gorevi: Seed akisi ve EF eslemesinin ayni varsayilan satirlari okumasini saglar.
public static class VehicleStatusConsts
{
    public const string TableName = "VehicleStatuses";
    public const string CodeIndex = "IX_VehicleStatuses_Code";

    // Realtime akisindan otomatik eklenen aracin baslangic durumu.
    public const int InServiceCode = 1;

    public static readonly IReadOnlyList<(int Id, string Name, int Code)> Definitions =
    [
        (1, "Hizmette", InServiceCode),
        (2, "Yedek", 2),
        (3, "Bakımda", 3),
        (4, "Hizmet dışı", 4)
    ];
}
