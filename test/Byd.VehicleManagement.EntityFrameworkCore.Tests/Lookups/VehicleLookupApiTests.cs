using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Controllers.Lookups;
using Byd.VehicleManagement.Permissions;
using Byd.VehicleManagement.Services.Lookups;
using Xunit;

namespace Byd.VehicleManagement.EntityFrameworkCore.Lookups;

public class VehicleLookupApiTests
{
    [Fact]
    public void EachLookupHasItsOwnControllerAndServiceWithFiveCrudActions()
    {
        var endpoints = new (Type Controller, Type Service, string Path)[]
        {
            (typeof(TransportModeController), typeof(ITransportModeAppService), VehicleLookupRoutes.TransportModes),
            (typeof(FuelTypeController), typeof(IFuelTypeAppService), VehicleLookupRoutes.FuelTypes),
            (typeof(VehicleStatusController), typeof(IVehicleStatusAppService), VehicleLookupRoutes.VehicleStatuses),
            (typeof(WheelchairAccessibleController), typeof(IWheelchairAccessibleAppService), VehicleLookupRoutes.WheelchairAccessibles),
            (typeof(EquipmentTypeController), typeof(IEquipmentTypeAppService), VehicleLookupRoutes.EquipmentTypes)
        };

        foreach (var (controller, service, path) in endpoints)
        {
            Assert.Equal(VehicleLookupRoutes.Root + "/" + path, controller.GetCustomAttribute<RouteAttribute>()?.Template);
            Assert.True(typeof(IVehicleLookupCrudAppService).IsAssignableFrom(service));
            Assert.Equal(5, controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Count(method => method.DeclaringType?.IsGenericType == true &&
                    method.DeclaringType.GetGenericTypeDefinition() == typeof(VehicleLookupControllerBase<>)));
            AssertAction(controller, nameof(IVehicleLookupCrudAppService.CreateAsync), typeof(HttpPostAttribute),
                VehicleManagementPermissions.Lookups.Manage);
            AssertAction(controller, nameof(IVehicleLookupCrudAppService.GetAsync), typeof(HttpGetAttribute),
                VehicleManagementPermissions.Lookups.View, VehicleLookupRoutes.ById);
            AssertAction(controller, nameof(IVehicleLookupCrudAppService.GetListAsync), typeof(HttpGetAttribute),
                VehicleManagementPermissions.Lookups.View);
            AssertAction(controller, nameof(IVehicleLookupCrudAppService.UpdateAsync), typeof(HttpPutAttribute),
                VehicleManagementPermissions.Lookups.Manage, VehicleLookupRoutes.ById);
            AssertAction(controller, nameof(IVehicleLookupCrudAppService.DeleteAsync), typeof(HttpDeleteAttribute),
                VehicleManagementPermissions.Lookups.Manage, VehicleLookupRoutes.ById);
        }
    }

    private static void AssertAction(Type controller, string name, Type verb, string permission, string? path = null)
    {
        var action = controller.GetMethod(name) ?? throw new InvalidOperationException(name);
        Assert.Equal(path, ((HttpMethodAttribute?)action.GetCustomAttribute(verb))?.Template);
        Assert.Equal(permission, action.GetCustomAttribute<AuthorizeAttribute>()?.Policy);
    }
}
