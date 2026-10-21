using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Data.Lookups;
using Byd.VehicleManagement.Dtos.VehicleModels;
using Byd.VehicleManagement.Dtos.VehicleTypes;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Services.Lookups;
using Byd.VehicleManagement.Services.VehicleModels;
using Byd.VehicleManagement.Services.VehicleTypes;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.VehicleTypes;

public class VehicleTypeCrudTests : VehicleManagementEntityFrameworkCoreTestBase
{
    private const int BusModeId = 4;
    private const int BatteryFuelId = 4;

    private static readonly Guid TenantId = Guid.NewGuid();
    private static readonly Guid OrganizationId = Guid.NewGuid();
    private static readonly Guid OtherOrganizationId = Guid.NewGuid();

    private ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();
    private ICurrentOrganization CurrentOrganization => ServiceProvider.GetRequiredService<ICurrentOrganization>();
    private IVehicleModelAppService ModelService => ServiceProvider.GetRequiredService<IVehicleModelAppService>();
    private IVehicleTypeAppService TypeService => ServiceProvider.GetRequiredService<IVehicleTypeAppService>();

    [Fact]
    public async Task TypeService_ReturnsRelatedNamesAndScopesCodesToOrganization()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        var model = await WithUnitOfWorkAsync(() => ModelService.CreateAsync(
            new CreateVehicleModelDto { Manufacturer = "Solaris", Name = "Urbino 18 electric" }));

        using (CurrentOrganization.Change(OrganizationId))
        {
            var created = await WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(model.Id, "U18E-48")));
            Assert.Equal(("Urbino 18 electric", "Otobüs", "Elektrik (batarya)"),
                (created.VehicleModelName, created.TransportModeName, created.FuelTypeName));
            var read = await WithUnitOfWorkAsync(() => TypeService.GetAsync(created.Id));
            Assert.Equal(created.VehicleModelName, read.VehicleModelName);
            var duplicate = await Assert.ThrowsAsync<BusinessException>(() =>
                WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(model.Id, "U18E-48"))));
            Assert.Equal(GeneralExceptionCodes.AlreadyExists, duplicate.Code);
        }

        using (CurrentOrganization.Change(OtherOrganizationId))
        {
            await WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(model.Id, "U18E-48")));
            var page = await WithUnitOfWorkAsync(() =>
                TypeService.GetFilteredListAsync(new DataTableListInput<VehicleTypeFilter>()));
            Assert.Equal(1, page.TotalCount);
            Assert.Equal("Urbino 18 electric", page.Items[0].VehicleModelName);
        }
    }

    [Fact]
    public async Task TypeService_UpdatesRelationsAndDeletes()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);
        var model = await WithUnitOfWorkAsync(() => ModelService.CreateAsync(
            new CreateVehicleModelDto { Manufacturer = "Solaris", Name = "Trollino 12" }));
        var created = await WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(model.Id, "TROL-12")));

        var updated = await WithUnitOfWorkAsync(() => TypeService.UpdateAsync(created.Id, new UpdateVehicleTypeDto
        {
            TransportModeId = 9, FuelTypeId = 5, Code = "TROL-12", Name = "Troleybüs 12 m", SeatingCapacity = 28,
            StandingCapacity = 62, LowFloor = true
        }));
        Assert.Null(updated.VehicleModelId);
        Assert.Null(updated.VehicleModelName);
        Assert.Equal(("Troleybüs", "Elektrik (havai hat)"), (updated.TransportModeName, updated.FuelTypeName));

        await WithUnitOfWorkAsync(() => TypeService.DeleteAsync(created.Id));
        await Assert.ThrowsAsync<BusinessException>(() => WithUnitOfWorkAsync(() => TypeService.GetAsync(created.Id)));
    }

    [Fact]
    public async Task TypeService_RejectsUnknownRelationsAndMissingOrganization()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        var withoutOrganization = await Assert.ThrowsAsync<BusinessException>(() =>
            WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(null, "BUS-12"))));
        Assert.Equal(VehicleTypeExceptionCodes.ScopeRequired, withoutOrganization.Code);

        using var organization = CurrentOrganization.Change(OrganizationId);
        var unknownModel = await Assert.ThrowsAsync<BusinessException>(() =>
            WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(Guid.NewGuid(), "BUS-12"))));
        Assert.Equal(VehicleTypeExceptionCodes.VehicleModelNotFound, unknownModel.Code);
        var unknownMode = NewType(null, "BUS-12");
        unknownMode.TransportModeId = 999;
        var unknownModeError = await Assert.ThrowsAsync<BusinessException>(() =>
            WithUnitOfWorkAsync(() => TypeService.CreateAsync(unknownMode)));
        Assert.Equal(VehicleTypeExceptionCodes.TransportModeNotFound, unknownModeError.Code);
    }

    [Fact]
    public async Task LookupUsedByAType_CannotBeDeletedFromHost()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using (CurrentTenant.Change(TenantId))
        using (CurrentOrganization.Change(OrganizationId))
        {
            await WithUnitOfWorkAsync(() => TypeService.CreateAsync(NewType(null, "BUS-12")));
        }

        var fuelTypes = ServiceProvider.GetRequiredService<IFuelTypeAppService>();
        var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            WithUnitOfWorkAsync(() => fuelTypes.DeleteAsync(BatteryFuelId)));
        Assert.Equal(GeneralExceptionCodes.RelationalDataDelete, exception.Code);
    }

    private static CreateVehicleTypeDto NewType(Guid? vehicleModelId, string code) => new()
    {
        VehicleModelId = vehicleModelId, TransportModeId = BusModeId, FuelTypeId = BatteryFuelId, Code = code,
        Name = "Urbino 18 electric - 48 koltuk", SeatingCapacity = 48, StandingCapacity = 97,
        WheelchairPlaceCapacity = 1, LowFloor = true, HasLiftOrRamp = true, Length = 18.0m
    };

    private async Task SeedLookupsAsync()
    {
        await ServiceProvider.GetRequiredService<TransportModeDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<FuelTypeDataSeeder>().SeedAsync(new DataSeedContext());
    }
}
