using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.Entities.Lookups;
using Byd.VehicleManagement.Interfaces.Lookups;
using Byd.VehicleManagement.Managers.Lookups;
using Byd.VehicleManagement.Mappers.Lookups;
using Byd.VehicleManagement.Models.Lookups;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.Lookups;

public class VehicleLookupTests
{
    [Fact]
    public async Task ManagerAndMapper_CreateAndUpdateTransportMode()
    {
        var repository = Substitute.For<IVehicleLookupRepository<TransportMode>>();
        var manager = new TransportModeManager(repository);
        var mapper = new TransportModeMapper();

        var entity = await manager.CreateAsync(mapper.Map(new CreateVehicleLookupDto
        {
            Code = 25,
            Name = "  Minibüs  "
        }));
        Assert.Equal((0, 25, "Minibüs"), (entity.Id, entity.Code, entity.Name));
        Assert.Equal((0, 25, "Minibüs"),
            (mapper.Map(entity).Id, mapper.Map(entity).Code, mapper.Map(entity).Name));

        await manager.UpdateAsync(entity, new VehicleLookupUpdateModel { Code = 26, Name = "  Yeni ad  " });
        Assert.Equal((26, "Yeni ad"), (entity.Code, entity.Name));
    }

    [Fact]
    public async Task Migrations_CreateFiveEmptyLookupTablesWithUniqueCodes()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = new VehicleManagementDbContext(
            new DbContextOptionsBuilder<VehicleManagementDbContext>().UseSqlite(connection).Options);

        await context.Database.MigrateAsync();
        Assert.Equal(context.Database.GetMigrations().Count(), (await context.Database.GetAppliedMigrationsAsync()).Count());
        Assert.False(await context.TransportModes.AnyAsync());
        Assert.False(await context.FuelTypes.AnyAsync());
        Assert.False(await context.VehicleStatuses.AnyAsync());
        Assert.False(await context.WheelchairAccessibles.AnyAsync());
        Assert.False(await context.EquipmentTypes.AnyAsync());
        var entities = context.Model.GetEntityTypes()
            .Where(entity => typeof(VehicleLookup).IsAssignableFrom(entity.ClrType)).ToList();
        Assert.Equal(5, entities.Count);
        Assert.All(entities, entity =>
        {
            Assert.Equal("vehicle_management", entity.GetSchema());
            Assert.Contains(entity.GetIndexes(), index => index.IsUnique &&
                index.Properties.Single().Name == nameof(VehicleLookup.Code));
        });
    }
}
