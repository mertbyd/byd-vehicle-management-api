using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Data.Lookups;
using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Services.Lookups;
using Volo.Abp.Data;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.Lookups;

public class VehicleLookupDataSeederTests : VehicleManagementEntityFrameworkCoreTestBase
{
    [Fact]
    public async Task Seeder_AddsDefaultsOnceAndKeepsUserChanges()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var service = ServiceProvider.GetRequiredService<ITransportModeAppService>();
            var seeder = ServiceProvider.GetRequiredService<TransportModeDataSeeder>();
            await seeder.SeedAsync(new DataSeedContext());

            var modes = await service.GetListAsync();
            Assert.Equal(TransportModeConsts.Definitions.Count, modes.Count);
            var bus = modes.Single(mode => mode.Code == 3);
            Assert.Equal("Otobüs", bus.Name);

            await service.UpdateAsync(bus.Id, new UpdateVehicleLookupDto { Code = 3, Name = "Şehir otobüsü" });
            await seeder.SeedAsync(new DataSeedContext());

            var reseeded = await service.GetListAsync();
            Assert.Equal(TransportModeConsts.Definitions.Count, reseeded.Count);
            Assert.Equal("Şehir otobüsü", reseeded.Single(mode => mode.Code == 3).Name);
        });
    }
}
