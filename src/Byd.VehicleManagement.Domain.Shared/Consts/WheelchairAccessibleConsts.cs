using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: WheelchairAccessible tablosunun kalici kimliklerini ve kodlarini tanimlar.
// sistemdeki gorevi: Seed akisi ve EF eslemesinin ayni varsayilan satirlari okumasini saglar.
public static class WheelchairAccessibleConsts
{
    public const string TableName = "WheelchairAccessibles";
    public const string CodeIndex = "IX_WheelchairAccessibles_Code";

    public static readonly IReadOnlyList<(int Id, string Name, int Code)> Definitions =
    [
        (1, "Bilgi yok", 0),
        (2, "Erişilebilir", 1),
        (3, "Erişilemez", 2)
    ];
}
