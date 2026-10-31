# Byd.VehicleManagement

Arac filosunu tasiyan ABP DDD modulu: arac lookup'lari, uretici modelleri, organizasyona ait arac tipleri, araclar ve
arac donanimlari. Tablolar `vehicle_management` semasinda durur.

## Paketler

Tuketen projede her katman ayni adli katman paketini referans alir ve o katmanin modulunu `[DependsOn]` ile ekler:

| Paket | Modul |
|---|---|
| `Byd.VehicleManagement.Domain.Shared` | `VehicleManagementDomainSharedModule` |
| `Byd.VehicleManagement.Domain` | `VehicleManagementDomainModule` |
| `Byd.VehicleManagement.Application.Contracts` | `VehicleManagementApplicationContractsModule` |
| `Byd.VehicleManagement.Application` | `VehicleManagementApplicationModule` |
| `Byd.VehicleManagement.EntityFrameworkCore` | `VehicleManagementEntityFrameworkCoreModule` |
| `Byd.VehicleManagement.HttpApi` | `VehicleManagementHttpApiModule` |
| `Byd.VehicleManagement.HttpApi.Client` | `VehicleManagementHttpApiClientModule` |

Modul `Byd.AuthManagement` ve `Byd.LogManagement` paketlerine baglidir; onlarla birlikte file ve core da
gelir, hepsinin host sozlesmesi gecerlidir.

## Kapsam

- Lookup'lar host kapsamindadir; varsayilan satirlari host acilisinda seeder'lar yazar. Ulasim modu kodu GTFS
  `route_type` ile aynidir.
- Uretici modeli tenant, arac tipi, arac ve donanim organizasyon kapsamindadir.
- Aracin kimligi organizasyonda isletmeci (`GtfsAgencyId`) ile birlikte GTFS-RT `vehicle.id`'dir. Plaka tekil
  sayilmaz, kaynaktaki haliyle tutulur.

## Realtime girisi

Realtime modulu araci `IVehicleAppService.FindOrRegisterRealtimeAsync` ile surec icinden bulur ya da kaydeder; bu metot
HTTP'ye acilmaz. Girdi GTFS standardinin degerleriyle gelir: `RouteType` (temel ya da genisletilmis `route_type`),
GTFS-RT `wheelchair_accessible` degeri, `GtfsAgencyId`, `GtfsVehicleId`, `Label` ve `RegistrationNumber`. Arac
organizasyonun cache'teki listesinde aranir; yoksa tipsiz, hizmette ve Realtime kaynakli olarak eklenir.

## Host baglantisi

- Baglanti dizesi adi `VehicleManagement`'tir; tanimli degilse `Default`'a duser.
- Modulun migration'lari `VehicleManagement.EntityFrameworkCore` paketindedir.
