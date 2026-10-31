# Byd.VehicleManagement

[![NuGet](https://img.shields.io/nuget/v/Byd.VehicleManagement.Domain?label=Byd.VehicleManagement)](https://www.nuget.org/packages/Byd.VehicleManagement.Domain)
![ABP](https://img.shields.io/badge/ABP-10.3-blue)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![GTFS](https://img.shields.io/badge/uyumlu-GTFS%20%2F%20GTFS--RT-0B8F50)

Toplu taşıma filosunun modülü. Araç lookup'larını, üretici modellerini, organizasyona ait araç tiplerini, araçları ve araç
donanımlarını tutar. Araç kimliği GTFS-RT ile uyumludur: canlı akışta görülen bir araç, kayıtlı değilse kendiliğinden
eklenir.

## İçindekiler

- [Platformdaki yeri](#platformdaki-yeri)
- [Bir bakışta](#bir-bakışta)
- [Veri modeli](#veri-modeli)
- [Özellikler](#özellikler)
- [HTTP uçları](#http-uçları)
- [Olaylar](#olaylar)
- [Yetkiler](#yetkiler)
- [Hata kodları](#hata-kodları)
- [Bir modülde kullanmak](#bir-modülde-kullanmak)
- [Geliştirme](#geliştirme)

## Platformdaki yeri

```mermaid
flowchart LR
    AUTH[Byd.AuthManagement<br/>log, file, core ile] --> VEH[Byd.VehicleManagement<br/><b>bu repo</b>]
    RT[Byd.GtfsRealtimeManagement] -- "VehicleRegistrationRequestedEto" --> VEH
    VEH -- "VehicleEto" --> RT
    style VEH fill:#6a48f5,color:#fff,stroke:#4a2fd0
```

## Bir bakışta

| | |
|---|---|
| Paket ailesi | `Byd.VehicleManagement.*` |
| Sürüm | `0.1.0` (`common.props`) |
| Bağımlılık | `Byd.AuthManagement` 0.1.9, `Byd.LogManagement` 0.1.6 (file ve core onlarla gelir) |
| Veritabanı | PostgreSQL, `vehicle_management` şeması; bağlantı dizesi `VehicleManagement` (yoksa `Default`) |
| Ek paket | `Byd.VehicleManagement.Consumer`: başka modüllerden gelen olayların handler'ları |

## Veri modeli

```mermaid
erDiagram
    VehicleModels ||--o{ VehicleTypes : "VehicleModelId"
    VehicleTypes ||--o{ Vehicles : "VehicleTypeId"
    Vehicles ||--o{ VehicleEquipments : "VehicleId"
    TransportModes ||--o{ VehicleTypes : ""
    FuelTypes ||--o{ VehicleTypes : ""
    VehicleStatuses ||--o{ Vehicles : ""
    RegistrationSources ||--o{ Vehicles : ""
    WheelchairAccessibles ||--o{ Vehicles : ""
    EquipmentTypes ||--o{ VehicleEquipments : ""
    VehicleModels {
        guid Id PK
        guid TenantId
        string Manufacturer
        string Name
    }
    VehicleTypes {
        guid Id PK
        guid OrganizationId
        string Code
        string Name
        int SeatingCapacity
        int StandingCapacity
        int WheelchairPlaceCapacity
        int BicycleCapacity
        bool LowFloor
        bool HasLiftOrRamp
        decimal Length
    }
    Vehicles {
        guid Id PK
        guid OrganizationId
        string GtfsAgencyId
        string GtfsVehicleId
        string Label
        string RegistrationNumber
        date InServiceFrom
        date InServiceUntil
    }
    VehicleEquipments {
        guid Id PK
        guid VehicleId FK
        int EquipmentTypeId FK
        int Units
    }
```

| Tablo | Kapsam | Ne tutar |
|---|---|---|
| `TransportModes` | Host (lookup) | Ulaşım modu; kodu GTFS `route_type` ile aynıdır |
| `FuelTypes` | Host (lookup) | Yakıt türü |
| `VehicleStatuses` | Host (lookup) | Araç durumu |
| `RegistrationSources` | Host (lookup) | Aracın kaydedildiği kaynak; canlı akıştan gelenler "Realtime" kaynaklıdır |
| `WheelchairAccessibles` | Host (lookup) | GTFS-RT `wheelchair_accessible` değerleri |
| `EquipmentTypes` | Host (lookup) | Donanım türü |
| `VehicleModels` | Tenant | Üretici ve model adı |
| `VehicleTypes` | Organizasyon | Kapasite, alçak taban, rampa, uzunluk; mod ve yakıt türü |
| `Vehicles` | Organizasyon | Araçlar |
| `VehicleEquipments` | Organizasyon | Aracın donanımları ve adetleri |

Lookup'ların varsayılan satırları host açılışında seed edilir.

## Özellikler

### 1. Lookup yönetimi

Altı lookup tek bir taban controller'dan aynı uç kümesini alır: ekleme, tekil okuma, liste, güncelleme ve silme. Lookup
satırı `Id`, `Code` ve `Name` taşır; kod tekildir.

### 2. Kapsam katmanları

| Kayıt | Kim görür |
|---|---|
| Lookup'lar | Herkes (host kapsamı) |
| Üretici modeli | Tenant'taki herkes |
| Araç tipi, araç, donanım | Yalnız kendi organizasyonu |

Organizasyon kapsamı token'dan gelir; organizasyonsuz bir istek araç yazamaz.

### 3. GTFS uyumlu araç kimliği

Aracın kimliği organizasyonda işletmeci (`GtfsAgencyId`) ile birlikte GTFS-RT `vehicle.id` değeridir (`GtfsVehicleId`).
Plaka tekil sayılmaz, kaynaktaki haliyle tutulur. Aracın hizmet aralığı `InServiceFrom` ve `InServiceUntil` ile tutulur.

### 4. Realtime'dan otomatik kayıt

Canlı akışta görülen ama kayıtlı olmayan araç iki yoldan eklenebilir:

| Yol | Nasıl |
|---|---|
| Olay | Realtime modülü `VehicleRegistrationRequestedEto` yayınlar; `Byd.VehicleManagement.Consumer` paketindeki handler aracı olayın kapsamında kaydeder |
| Süreç içi çağrı | `IVehicleAppService.FindOrRegisterRealtimeAsync` (HTTP'ye açılmaz) |

```mermaid
sequenceDiagram
    participant RT as Realtime modülü
    participant MQ as RabbitMQ
    participant H as Consumer handler'ı
    participant DB as Vehicles
    RT->>MQ: VehicleRegistrationRequestedEto (GtfsVehicleId, RouteType, ...)
    MQ->>H: Olay alınır
    H->>DB: Organizasyonun araç listesinde ara
    alt kayıtlı değil
        H->>DB: Tipsiz, hizmette, "Realtime" kaynaklı araç ekle
    else zaten kayıtlı
        H-->>MQ: Sessizce tamamla
    end
```

Girdi GTFS standardının değerleriyle gelir: `RouteType` (temel ya da genişletilmiş `route_type`), `wheelchair_accessible`,
`GtfsAgencyId`, `GtfsVehicleId`, `Label` ve `RegistrationNumber`. Aynı olayın tekrar teslimi ikinci bir araç yazmaz.

### 5. Önbellekli araç listesi

Organizasyonun araç listesi önbellekte tutulur ve araç satırı değiştiğinde silinir. Realtime eşlemesi her mesajda
veritabanına gitmez.

### 6. Araç değişikliği olayı

Araç eklendiğinde, güncellendiğinde ya da silindiğinde ABP entity olayı `VehicleEto` ile dağıtık olarak yayınlanır.
Diğer modüller akıştaki GTFS-RT kimliğini araç kaydına bu olayla eşler.

## HTTP uçları

Her kaynak aynı uç kümesini taşır:

| Yöntem | Yol | Ne yapar |
|---|---|---|
| `POST` | `{kaynak}` | Ekler |
| `GET` | `{kaynak}/{id}` | Tek kayıt |
| `GET` | `{kaynak}/filtered-list` | Filtreli, sıralı, sayfalı liste |
| `PUT` | `{kaynak}/{id}` | Günceller |
| `DELETE` | `{kaynak}/{id}` | Siler |

| Kaynak | Kök yol |
|---|---|
| Araçlar | `api/vehicle-management/vehicles` |
| Araç tipleri | `api/vehicle-management/vehicle-types` |
| Üretici modelleri | `api/vehicle-management/vehicle-models` |
| Araç donanımları | `api/vehicle-management/vehicle-equipments` |
| Lookup'lar | `api/vehicle-management/lookups/{transport-modes, fuel-types, vehicle-statuses, wheelchair-accessibles, equipment-types, registration-sources}` (liste için `GET` kökü, kimlik `int`) |

## Olaylar

| Olay | Ad | Yön | Ne zaman |
|---|---|---|---|
| `VehicleEto` | `VehicleManagement.Vehicle` | Bu modül yayınlar | Araç eklenince, güncellenince, silinince; `Id`, `TenantId`, `OrganizationId`, `GtfsAgencyId`, `GtfsVehicleId` |
| `VehicleRegistrationRequestedEto` | `VehicleManagement.Vehicles.RegistrationRequested` | Bu modül dinler | Realtime akışında kayıtsız araç görülünce |

## Yetkiler

| İzin | Ne verir |
|---|---|
| `VehicleManagement.Lookups.View` / `.Manage` | Lookup'ları görmek / yönetmek |
| `VehicleManagement.VehicleModels.View` / `.Manage` | Üretici modelleri |
| `VehicleManagement.VehicleTypes.View` / `.Manage` | Araç tipleri |
| `VehicleManagement.Vehicles.View` / `.Manage` | Araçlar |
| `VehicleManagement.VehicleEquipments.View` / `.Manage` | Araç donanımları |

## Hata kodları

| Aile | Kapsadığı durumlar |
|---|---|
| `VehicleExceptionCodes` | Kapsam yok, mod/durum zorunlu, GTFS kimliği eksik/uzun, hizmet bitişi başlangıçtan önce, tip/durum bulunamadı |
| `VehicleTypeExceptionCodes` | Kod/ad eksik ya da uzun, mod/yakıt zorunlu, geçersiz kapasite ve uzunluk |
| `VehicleModelExceptionCodes` | Tenant zorunlu, üretici/ad eksik ya da uzun |
| `VehicleEquipmentExceptionCodes` | Araç/donanım türü zorunlu ya da bulunamadı, geçersiz adet |
| `VehicleLookupExceptionCodes` | Ad zorunlu, kod geçersiz ya da tekrarlı |

Kodlar `VehicleManagement:` önekiyle gelir.

## Bir modülde kullanmak

| Tüketen katman | Paket | Modül sınıfı |
|---|---|---|
| `<Modül>.Domain.Shared` | `Byd.VehicleManagement.Domain.Shared` | `VehicleManagementDomainSharedModule` |
| `<Modül>.Domain` | `Byd.VehicleManagement.Domain` | `VehicleManagementDomainModule` |
| `<Modül>.Application.Contracts` | `Byd.VehicleManagement.Application.Contracts` | `VehicleManagementApplicationContractsModule` |
| `<Modül>.Application` | `Byd.VehicleManagement.Application` | `VehicleManagementApplicationModule` |
| `<Modül>.EntityFrameworkCore` | `Byd.VehicleManagement.EntityFrameworkCore` | `VehicleManagementEntityFrameworkCoreModule` |
| `<Modül>.HttpApi` | `Byd.VehicleManagement.HttpApi` | `VehicleManagementHttpApiModule` |
| Olayları işleyecek host | `Byd.VehicleManagement.Consumer` | `VehicleManagementConsumerModule` |

Handler'lar ayrı pakettedir; modülü NuGet olarak alan bir host onları istemeden almaz. Yalnız olay sözleşmesi gereken bir
modüle `Domain.Shared` yeter.

**Host**

| Gereken | Neden |
|---|---|
| Core, log ve auth host sözleşmesi | Zincirin bütün modülleri |
| `ConnectionStrings:VehicleManagement` | Yoksa `Default` kullanılır |
| `RabbitMQ` bölümü | Realtime ile olay alışverişi |

Migration'lar `Byd.VehicleManagement.EntityFrameworkCore` paketindedir.

## Geliştirme

```bash
dotnet build Byd.VehicleManagement.slnx
dotnet test Byd.VehicleManagement.slnx
```

Host `host/Byd.VehicleManagement.HttpApi.Host` altındadır; repo kökünde bir `Dockerfile` vardır.
