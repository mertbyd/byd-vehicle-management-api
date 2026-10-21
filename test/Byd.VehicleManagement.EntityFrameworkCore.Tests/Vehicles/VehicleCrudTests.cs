using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Data.Lookups;
using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.Dtos.VehicleTypes;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Services.Vehicles;
using Byd.VehicleManagement.Services.VehicleTypes;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.Vehicles;

public class VehicleCrudTests : VehicleManagementEntityFrameworkCoreTestBase
{
    private const int BusModeId = 4;
    private const int TrolleybusModeId = 9;
    private const int InServiceStatusId = 1;
    private const int BusRouteType = 3;
    private const int ExtendedTrolleybusRouteType = 800;
    private const int RealtimeWheelchairInaccessible = 3;
    private const int RealtimeWheelchairNoValue = 0;

    private static readonly Guid TenantId = Guid.NewGuid();
    private static readonly Guid OrganizationId = Guid.NewGuid();
    private static readonly Guid OtherOrganizationId = Guid.NewGuid();

    private ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();
    private ICurrentOrganization CurrentOrganization => ServiceProvider.GetRequiredService<ICurrentOrganization>();
    private IVehicleTypeAppService TypeService => ServiceProvider.GetRequiredService<IVehicleTypeAppService>();
    private IVehicleAppService VehicleService => ServiceProvider.GetRequiredService<IVehicleAppService>();

    [Fact]
    public async Task VehicleService_ReturnsRelatedNamesUpdatesAndDeletes()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);
        var type = await CreateTypeAsync("U18E-48");

        var created = await WithUnitOfWorkAsync(() => VehicleService.CreateAsync(NewVehicle(type.Id, "3105", "BUS 105")));
        Assert.Equal(("U18E-48 tipi", "Otobüs", "Hizmette", "Manuel"),
            (created.VehicleTypeName, created.TransportModeName, created.VehicleStatusName, created.RegistrationSourceName));

        var update = ToUpdate(NewVehicle(type.Id, "3105", "BUS 205"));
        var updated = await WithUnitOfWorkAsync(() => VehicleService.UpdateAsync(created.Id, update));
        Assert.Equal(("BUS 205", "Manuel"), (updated.RegistrationNumber, updated.RegistrationSourceName));

        await WithUnitOfWorkAsync(() => VehicleService.DeleteAsync(created.Id));
        var page = await WithUnitOfWorkAsync(() => VehicleService.GetFilteredListAsync(new DataTableListInput<VehicleFilter>()));
        Assert.Equal(0, page.TotalCount);
    }

    [Fact]
    public async Task VehicleService_ScopesGtfsIdToOrganizationAndAllowsSharedPlate()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using (CurrentOrganization.Change(OrganizationId))
        {
            var type = await CreateTypeAsync("U18E-48");
            await WithUnitOfWorkAsync(() => VehicleService.CreateAsync(NewVehicle(type.Id, "3105", "BUS 105")));
            await AssertCodeAsync(VehicleExceptionCodes.GtfsVehicleIdAlreadyExists,
                () => VehicleService.CreateAsync(NewVehicle(type.Id, "3105", null)));
        }

        using (CurrentOrganization.Change(OtherOrganizationId))
        {
            var type = await CreateTypeAsync("U18E-48");
            await WithUnitOfWorkAsync(() => VehicleService.CreateAsync(NewVehicle(type.Id, "3105", null)));
            var sharedPlate = await WithUnitOfWorkAsync(() => VehicleService.CreateAsync(NewVehicle(type.Id, "4210", "BUS 105")));
            Assert.Equal("BUS 105", sharedPlate.RegistrationNumber);
        }
    }

    [Fact]
    public async Task VehicleService_RejectsForeignTypeAndModeMismatch()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        VehicleTypeDto otherType;
        using (CurrentOrganization.Change(OtherOrganizationId))
            otherType = await CreateTypeAsync("U18E-48");

        using var organization = CurrentOrganization.Change(OrganizationId);
        await AssertCodeAsync(VehicleExceptionCodes.VehicleTypeNotFound,
            () => VehicleService.CreateAsync(NewVehicle(otherType.Id, "3105", null)));
        var type = await CreateTypeAsync("BUS-12");
        var trolleybus = NewVehicle(type.Id, "1745", null);
        trolleybus.TransportModeId = TrolleybusModeId;
        await AssertCodeAsync(VehicleExceptionCodes.VehicleTypeTransportModeMismatch,
            () => VehicleService.CreateAsync(trolleybus));
    }

    [Fact]
    public async Task RealtimeService_RegistersUnknownVehicleOnceAndThenFindsIt()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);
        var seen = new RegisterRealtimeVehicleDto { RouteType = BusRouteType, GtfsVehicleId = " 3105 ", Label = "3105" };

        var registered = await WithUnitOfWorkAsync(() => VehicleService.FindOrRegisterRealtimeAsync(seen));
        Assert.Null(registered.VehicleTypeId);
        Assert.Equal(("3105", "Otobüs", "Hizmette"),
            (registered.GtfsVehicleId, registered.TransportModeName, registered.VehicleStatusName));
        var found = await WithUnitOfWorkAsync(() => VehicleService.FindOrRegisterRealtimeAsync(seen));
        Assert.Equal(registered.Id, found.Id);

        var vehicle = await WithUnitOfWorkAsync(() => VehicleService.GetAsync(registered.Id));
        Assert.Equal("Realtime", vehicle.RegistrationSourceName);
        var type = await CreateTypeAsync("U18E-48");
        var assigned = ToUpdate(NewVehicle(type.Id, "3105", null));
        await WithUnitOfWorkAsync(() => VehicleService.UpdateAsync(registered.Id, assigned));
        var refreshed = await WithUnitOfWorkAsync(() => VehicleService.FindOrRegisterRealtimeAsync(seen));
        Assert.Equal(("U18E-48 tipi", registered.Id), (refreshed.VehicleTypeName, refreshed.Id));
    }

    [Fact]
    public async Task RealtimeService_DoesNotSeeAnotherOrganizationsVehicle()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        var seen = new RegisterRealtimeVehicleDto { RouteType = BusRouteType, GtfsVehicleId = "3105" };
        VehicleRealtimeDto first;
        using (CurrentOrganization.Change(OrganizationId))
            first = await WithUnitOfWorkAsync(() => VehicleService.FindOrRegisterRealtimeAsync(seen));

        using (CurrentOrganization.Change(OtherOrganizationId))
        {
            var second = await WithUnitOfWorkAsync(() => VehicleService.FindOrRegisterRealtimeAsync(seen));
            Assert.NotEqual(first.Id, second.Id);
        }
    }

    [Fact]
    public async Task RealtimeService_SeparatesOperatorsAndKeepsPlateAsReceived()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);

        var atb = await RegisterAsync(new() { RouteType = BusRouteType, GtfsAgencyId = "ATB", GtfsVehicleId = "3172" });
        var kol = await RegisterAsync(new() { RouteType = BusRouteType, GtfsAgencyId = "KOL", GtfsVehicleId = "3172" });
        Assert.NotEqual(atb.Id, kol.Id);
        Assert.Equal(atb.Id,
            (await RegisterAsync(new() { RouteType = BusRouteType, GtfsAgencyId = "ATB", GtfsVehicleId = "3172" })).Id);

        var first = await RegisterAsync(new() { RouteType = BusRouteType, GtfsVehicleId = "2806", RegistrationNumber = "EC 5197" });
        var second = await RegisterAsync(new() { RouteType = BusRouteType, GtfsVehicleId = "2810", RegistrationNumber = "EC 5197" });
        Assert.Equal(("EC 5197", "EC 5197"), (first.RegistrationNumber, second.RegistrationNumber));
        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public async Task RealtimeService_TranslatesStandardValuesToLookupCodes()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);

        var trolleybus = await RegisterAsync(new()
        {
            RouteType = ExtendedTrolleybusRouteType, GtfsVehicleId = "1119", WheelchairAccessible = RealtimeWheelchairInaccessible
        });
        var unknown = await RegisterAsync(new()
        {
            RouteType = BusRouteType, GtfsVehicleId = "4216", WheelchairAccessible = RealtimeWheelchairNoValue
        });

        var trolleybusVehicle = await WithUnitOfWorkAsync(() => VehicleService.GetAsync(trolleybus.Id));
        Assert.Equal(("Troleybüs", "Erişilemez"), (trolleybusVehicle.TransportModeName, trolleybusVehicle.WheelchairAccessibleName));
        Assert.Null((await WithUnitOfWorkAsync(() => VehicleService.GetAsync(unknown.Id))).WheelchairAccessibleId);
        await AssertCodeAsync(VehicleExceptionCodes.TransportModeNotFound,
            () => VehicleService.FindOrRegisterRealtimeAsync(new() { RouteType = 1700, GtfsVehicleId = "9999" }));
    }

    [Fact]
    public async Task TypeUsedByAVehicle_CannotBeDeleted()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);
        var type = await CreateTypeAsync("U18E-48");
        await WithUnitOfWorkAsync(() => VehicleService.CreateAsync(NewVehicle(type.Id, "3105", null)));

        await AssertCodeAsync(GeneralExceptionCodes.RelationalDataDelete, () => TypeService.DeleteAsync(type.Id));
    }

    private Task<VehicleTypeDto> CreateTypeAsync(string code)
        => WithUnitOfWorkAsync(() => TypeService.CreateAsync(new CreateVehicleTypeDto
        {
            TransportModeId = BusModeId, FuelTypeId = 4, Code = code, Name = $"{code} tipi", SeatingCapacity = 48,
            StandingCapacity = 97
        }));

    private Task<VehicleRealtimeDto> RegisterAsync(RegisterRealtimeVehicleDto input)
        => WithUnitOfWorkAsync(() => VehicleService.FindOrRegisterRealtimeAsync(input));

    private async Task AssertCodeAsync(string code, Func<Task> action)
    {
        var exception = await Assert.ThrowsAsync<BusinessException>(() => WithUnitOfWorkAsync(action));
        Assert.Equal(code, exception.Code);
    }

    private static CreateVehicleDto NewVehicle(Guid? vehicleTypeId, string gtfsVehicleId, string? registrationNumber) => new()
    {
        VehicleTypeId = vehicleTypeId, TransportModeId = BusModeId, VehicleStatusId = InServiceStatusId,
        GtfsVehicleId = gtfsVehicleId, Label = gtfsVehicleId, RegistrationNumber = registrationNumber,
        InServiceFrom = new DateOnly(2024, 5, 1)
    };

    private static UpdateVehicleDto ToUpdate(CreateVehicleDto input) => new()
    {
        VehicleTypeId = input.VehicleTypeId, TransportModeId = input.TransportModeId, VehicleStatusId = input.VehicleStatusId,
        GtfsVehicleId = input.GtfsVehicleId, Label = input.Label, RegistrationNumber = input.RegistrationNumber,
        GtfsAgencyId = input.GtfsAgencyId, InServiceFrom = input.InServiceFrom
    };

    private async Task SeedLookupsAsync()
    {
        await ServiceProvider.GetRequiredService<TransportModeDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<FuelTypeDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<VehicleStatusDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<RegistrationSourceDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<WheelchairAccessibleDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<EquipmentTypeDataSeeder>().SeedAsync(new DataSeedContext());
    }
}
