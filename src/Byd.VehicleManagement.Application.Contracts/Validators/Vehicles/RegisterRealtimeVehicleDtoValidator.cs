using FluentValidation;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.Vehicles;

// islevi: Realtime bul-ya da-ekle girdisinin route_type, kimlik ve uzunluk kurallarini dogrular.
// sistemdeki gorevi: Feed'den gelen bozuk VehicleDescriptor'in arac olarak yazilmasini Manager'a ulasmadan engeller.
public sealed class RegisterRealtimeVehicleDtoValidator : AbstractValidator<RegisterRealtimeVehicleDto>
{
    // route_type, GTFS-RT kimligi, numara, plaka ve isletmeci kurallarini kaydeder.
    public RegisterRealtimeVehicleDtoValidator()
    {
        RuleFor(input => input.RouteType)
            .GreaterThanOrEqualTo(0).WithErrorCode(VehicleExceptionCodes.TransportModeRequired);
        RuleFor(input => input.GtfsVehicleId)
            .NotEmpty().WithErrorCode(VehicleExceptionCodes.GtfsVehicleIdRequired)
            .MaximumLength(VehicleConsts.MaxGtfsVehicleIdLength).WithErrorCode(VehicleExceptionCodes.GtfsVehicleIdTooLong);
        RuleFor(input => input.Label)
            .MaximumLength(VehicleConsts.MaxLabelLength).WithErrorCode(VehicleExceptionCodes.LabelTooLong);
        RuleFor(input => input.RegistrationNumber)
            .MaximumLength(VehicleConsts.MaxRegistrationNumberLength)
            .WithErrorCode(VehicleExceptionCodes.RegistrationNumberTooLong);
        RuleFor(input => input.GtfsAgencyId)
            .MaximumLength(VehicleConsts.MaxGtfsAgencyIdLength).WithErrorCode(VehicleExceptionCodes.GtfsAgencyIdTooLong);
    }
}
