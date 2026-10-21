using System.Collections.Generic;

namespace Byd.VehicleManagement.Consts;

// islevi: EquipmentType tablosunun kalici kimliklerini ve kodlarini tanimlar.
// sistemdeki gorevi: Seed akisi ve EF eslemesinin ayni varsayilan satirlari okumasini saglar.
public static class EquipmentTypeConsts
{
    public const string TableName = "EquipmentTypes";
    public const string CodeIndex = "IX_EquipmentTypes_Code";

    public static readonly IReadOnlyList<(int Id, string Name, int Code)> Definitions =
    [
        (1, "Wi-Fi", 1),
        (2, "Klima", 2),
        (3, "USB şarj", 3),
        (4, "Bisiklet aparatı", 4),
        (5, "Yolcu bilgi ekranı", 5),
        (6, "Sesli anons", 6),
        (7, "Kamera", 7),
        (8, "Yolcu sayıcı (APC)", 8)
    ];
}
