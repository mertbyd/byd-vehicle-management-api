using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Services.Lookups;
using Volo.Abp;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.Lookups;

public class VehicleLookupCrudTests : VehicleManagementEntityFrameworkCoreTestBase
{
    [Fact]
    public async Task TransportModeService_CreatesReadsUpdatesAndDeletes()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var service = ServiceProvider.GetRequiredService<ITransportModeAppService>();
            var created = await service.CreateAsync(new CreateVehicleLookupDto { Code = 25, Name = "Minibüs" });
            Assert.True(created.Id > 0);
            await Assert.ThrowsAsync<BusinessException>(() => service.CreateAsync(
                new CreateVehicleLookupDto { Code = 25, Name = "Tekrar" }));
            Assert.Equal("Minibüs", (await service.GetAsync(created.Id)).Name);
            Assert.Contains(await service.GetListAsync(), item => item.Id == created.Id);

            var updated = await service.UpdateAsync(created.Id,
                new UpdateVehicleLookupDto { Code = 26, Name = "Yeni ad" });
            Assert.Equal((26, "Yeni ad"), (updated.Code, updated.Name));

            await service.DeleteAsync(created.Id);
            Assert.DoesNotContain(await service.GetListAsync(), item => item.Id == created.Id);
        });
    }
}
