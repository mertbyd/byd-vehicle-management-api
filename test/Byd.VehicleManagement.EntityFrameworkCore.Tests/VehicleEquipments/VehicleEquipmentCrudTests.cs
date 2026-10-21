using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Interfaces;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Data.Lookups;
using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.ExceptionCodes;
using Byd.VehicleManagement.Services.VehicleEquipments;
using Byd.VehicleManagement.Services.Vehicles;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.VehicleEquipments;

public class VehicleEquipmentCrudTests : VehicleManagementEntityFrameworkCoreTestBase
{
    private const int BusModeId = 4;
    private const int InServiceStatusId = 1;
    private const int WifiEquipmentId = 1;
    private const int CameraEquipmentId = 7;

    private static readonly Guid TenantId = Guid.NewGuid();
    private static readonly Guid OrganizationId = Guid.NewGuid();
    private static readonly Guid OtherOrganizationId = Guid.NewGuid();

    private ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();
    private ICurrentOrganization CurrentOrganization => ServiceProvider.GetRequiredService<ICurrentOrganization>();
    private IVehicleAppService VehicleService => ServiceProvider.GetRequiredService<IVehicleAppService>();
    private IVehicleEquipmentAppService EquipmentService => ServiceProvider.GetRequiredService<IVehicleEquipmentAppService>();

    [Fact]
    public async Task EquipmentService_CreatesReadsUpdatesListsAndDeletes()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);
        var vehicle = await CreateVehicleAsync("3105");

        var created = await WithUnitOfWorkAsync(() => EquipmentService.CreateAsync(
            new CreateVehicleEquipmentDto { VehicleId = vehicle.Id, EquipmentTypeId = WifiEquipmentId, Units = 1 }));
        Assert.Equal(("3105", "Wi-Fi", 1), (created.VehicleGtfsVehicleId, created.EquipmentTypeName, created.Units));

        var updated = await WithUnitOfWorkAsync(() => EquipmentService.UpdateAsync(created.Id,
            new UpdateVehicleEquipmentDto { EquipmentTypeId = CameraEquipmentId, Units = 4 }));
        Assert.Equal(("Kamera", 4, vehicle.Id), (updated.EquipmentTypeName, updated.Units, updated.VehicleId));

        var page = await WithUnitOfWorkAsync(() => EquipmentService.GetFilteredListAsync(ByVehicle(vehicle.Id)));
        Assert.Equal(created.Id, page.Items.Single().Id);

        await WithUnitOfWorkAsync(() => EquipmentService.DeleteAsync(created.Id));
        Assert.Equal(0, (await WithUnitOfWorkAsync(() => EquipmentService.GetFilteredListAsync(ByVehicle(vehicle.Id)))).TotalCount);
    }

    [Fact]
    public async Task EquipmentService_RejectsDuplicateTypeAndAnotherOrganizationsVehicle()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        VehicleDto otherVehicle;
        using (CurrentOrganization.Change(OtherOrganizationId))
            otherVehicle = await CreateVehicleAsync("9001");

        using var organization = CurrentOrganization.Change(OrganizationId);
        var vehicle = await CreateVehicleAsync("3105");
        var wifi = new CreateVehicleEquipmentDto { VehicleId = vehicle.Id, EquipmentTypeId = WifiEquipmentId, Units = 1 };
        await WithUnitOfWorkAsync(() => EquipmentService.CreateAsync(wifi));

        await AssertCodeAsync(GeneralExceptionCodes.AlreadyExists, () => EquipmentService.CreateAsync(wifi));
        await AssertCodeAsync(VehicleEquipmentExceptionCodes.VehicleNotFound, () => EquipmentService.CreateAsync(
            new CreateVehicleEquipmentDto { VehicleId = otherVehicle.Id, EquipmentTypeId = WifiEquipmentId, Units = 1 }));
        await AssertCodeAsync(VehicleEquipmentExceptionCodes.EquipmentTypeNotFound, () => EquipmentService.CreateAsync(
            new CreateVehicleEquipmentDto { VehicleId = vehicle.Id, EquipmentTypeId = 999, Units = 1 }));
    }

    [Fact]
    public async Task DeletingAVehicle_DeletesItsEquipment()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        using var organization = CurrentOrganization.Change(OrganizationId);
        var vehicle = await CreateVehicleAsync("3105");
        await WithUnitOfWorkAsync(() => EquipmentService.CreateAsync(
            new CreateVehicleEquipmentDto { VehicleId = vehicle.Id, EquipmentTypeId = WifiEquipmentId, Units = 1 }));

        await WithUnitOfWorkAsync(() => VehicleService.DeleteAsync(vehicle.Id));

        Assert.Equal(0, (await WithUnitOfWorkAsync(() => EquipmentService.GetFilteredListAsync(ByVehicle(vehicle.Id)))).TotalCount);
    }

    private Task<VehicleDto> CreateVehicleAsync(string gtfsVehicleId)
        => WithUnitOfWorkAsync(() => VehicleService.CreateAsync(new CreateVehicleDto
        {
            TransportModeId = BusModeId, VehicleStatusId = InServiceStatusId, GtfsVehicleId = gtfsVehicleId,
            InServiceFrom = new DateOnly(2024, 5, 1)
        }));

    private static DataTableListInput<VehicleEquipmentFilter> ByVehicle(Guid vehicleId) => new()
    {
        Filters = new VehicleEquipmentFilter
        {
            VehicleId = new DataTableFilterItem { Operator = DataTableFilterOperator.Equal, Value = vehicleId.ToString() }
        }
    };

    private async Task AssertCodeAsync(string code, Func<Task> action)
    {
        var exception = await Assert.ThrowsAsync<BusinessException>(() => WithUnitOfWorkAsync(action));
        Assert.Equal(code, exception.Code);
    }

    private async Task SeedLookupsAsync()
    {
        await ServiceProvider.GetRequiredService<TransportModeDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<VehicleStatusDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<RegistrationSourceDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<EquipmentTypeDataSeeder>().SeedAsync(new DataSeedContext());
    }
}
