using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Byd.CoreExtensions.Querying;
using Byd.CoreExtensions.Repositories;
using Byd.VehicleManagement.Entities.VehicleEquipments;
using Byd.VehicleManagement.Interfaces.VehicleEquipments;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Byd.VehicleManagement.EntityFrameworkCore;

// islevi: VehicleEquipment repository sozlesmesini core EF tabanina baglar; arac ve donanim turu navigation'larini
// yukler.
// sistemdeki gorevi: Cevap DTO'sunun aracin GTFS-RT kimligini ve tur adini tasiyabilmesi icin Include'u tek yerde tutar.
[ExposeServices(typeof(IVehicleEquipmentRepository))]
public class EfCoreVehicleEquipmentRepository(IDbContextProvider<VehicleManagementDbContext> dbContextProvider)
    : EfExtendedRepository<VehicleManagementDbContext, VehicleEquipment, Guid>(dbContextProvider),
        IVehicleEquipmentRepository
{
    // Tekil okumalarda bagli kayit degerlerinin dolu gelmesini saglar.
    public override async Task<IQueryable<VehicleEquipment>> WithDetailsAsync()
        => IncludeRelations(await GetQueryableAsync());

    // Liste sorgusunda da ayni navigation'lari yukler; filtre, siralama ve sayfalama tabanda kalir.
    protected override async Task<IQueryable<VehicleEquipment>> CreateFilteredQueryAsync<TFiltering>(
        IDataTablePagingProperty<TFiltering> pagingProperty)
        => IncludeRelations(await base.CreateFilteredQueryAsync(pagingProperty));

    // Cevabin arac kimligini ve donanim turu adini tasiyan navigation'lari ekler.
    private static IQueryable<VehicleEquipment> IncludeRelations(IQueryable<VehicleEquipment> query)
        => query.Include(equipment => equipment.Vehicle)
            .Include(equipment => equipment.EquipmentType);
}
