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
using Byd.VehicleManagement.Services.VehicleModels;
using Byd.VehicleManagement.Services.VehicleTypes;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.VehicleModels;

public class VehicleModelCrudTests : VehicleManagementEntityFrameworkCoreTestBase
{
    private static readonly Guid TenantId = Guid.NewGuid();

    private ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();
    private ICurrentOrganization CurrentOrganization => ServiceProvider.GetRequiredService<ICurrentOrganization>();
    private IVehicleModelAppService ModelService => ServiceProvider.GetRequiredService<IVehicleModelAppService>();
    private IVehicleTypeAppService TypeService => ServiceProvider.GetRequiredService<IVehicleTypeAppService>();

    [Fact]
    public async Task ModelService_CreatesReadsUpdatesListsAndDeletesInTenant()
    {
        using var tenant = CurrentTenant.Change(TenantId);
        await WithUnitOfWorkAsync(async () =>
        {
            var created = await ModelService.CreateAsync(
                new CreateVehicleModelDto { Manufacturer = " Solaris ", Name = "Urbino 18 electric" });
            Assert.Equal(("Solaris", "Urbino 18 electric"), (created.Manufacturer, created.Name));
            var duplicate = await Assert.ThrowsAsync<BusinessException>(() => ModelService.CreateAsync(
                new CreateVehicleModelDto { Manufacturer = "Solaris", Name = "Urbino 18 electric" }));
            Assert.Equal(GeneralExceptionCodes.AlreadyExists, duplicate.Code);

            var updated = await ModelService.UpdateAsync(created.Id,
                new UpdateVehicleModelDto { Manufacturer = "Solaris", Name = "Urbino 18 hydrogen" });
            Assert.Equal("Urbino 18 hydrogen", (await ModelService.GetAsync(updated.Id)).Name);
            Assert.Equal(1, (await ModelService.GetFilteredListAsync(new DataTableListInput<VehicleModelFilter>())).TotalCount);

            await ModelService.DeleteAsync(created.Id);
            await Assert.ThrowsAsync<BusinessException>(() => ModelService.GetAsync(created.Id));
        });
    }

    [Fact]
    public async Task ModelService_RejectsCreateWithoutTenant()
    {
        var exception = await Assert.ThrowsAsync<BusinessException>(() => WithUnitOfWorkAsync(() =>
            ModelService.CreateAsync(new CreateVehicleModelDto { Manufacturer = "Solaris", Name = "Trollino 12" })));
        Assert.Equal(VehicleModelExceptionCodes.TenantRequired, exception.Code);
    }

    [Fact]
    public async Task ModelUsedByAnotherOrganizationsType_CannotBeDeleted()
    {
        await WithUnitOfWorkAsync(SeedLookupsAsync);
        using var tenant = CurrentTenant.Change(TenantId);
        var model = await WithUnitOfWorkAsync(() => ModelService.CreateAsync(
            new CreateVehicleModelDto { Manufacturer = "Solaris", Name = "Urbino 18 electric" }));
        using (CurrentOrganization.Change(Guid.NewGuid()))
        {
            await WithUnitOfWorkAsync(() => TypeService.CreateAsync(new CreateVehicleTypeDto
            {
                VehicleModelId = model.Id, TransportModeId = 4, FuelTypeId = 4, Code = "U18E-48", Name = "Urbino 48"
            }));
        }

        using (CurrentOrganization.Change(Guid.NewGuid()))
        {
            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                WithUnitOfWorkAsync(() => ModelService.DeleteAsync(model.Id)));
            Assert.Equal(GeneralExceptionCodes.RelationalDataDelete, exception.Code);
        }
    }

    private async Task SeedLookupsAsync()
    {
        await ServiceProvider.GetRequiredService<TransportModeDataSeeder>().SeedAsync(new DataSeedContext());
        await ServiceProvider.GetRequiredService<FuelTypeDataSeeder>().SeedAsync(new DataSeedContext());
    }
}
