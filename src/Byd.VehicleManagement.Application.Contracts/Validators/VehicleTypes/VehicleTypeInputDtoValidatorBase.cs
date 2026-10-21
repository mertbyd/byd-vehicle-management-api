using FluentValidation;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.VehicleTypes;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.VehicleTypes;

// islevi: Arac tipi alanlarinin zorunluluk, uzunluk ve aralik kurallarini tek yerde dogrular.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ayni sekil sozlesmesinden gecmesini saglar; baglarin
// var olup olmadigi Manager'da kontrol edilir.
public abstract class VehicleTypeInputDtoValidatorBase<TDto> : AbstractValidator<TDto>
    where TDto : VehicleTypeInputDto
{
    // Kod, ad, lookup secimi, kapasite ve uzunluk kurallarini kaydeder.
    protected VehicleTypeInputDtoValidatorBase()
    {
        RuleFor(input => input.TransportModeId)
            .GreaterThan(0).WithErrorCode(VehicleTypeExceptionCodes.TransportModeRequired);
        RuleFor(input => input.FuelTypeId)
            .GreaterThan(0).WithErrorCode(VehicleTypeExceptionCodes.FuelTypeRequired);
        RuleFor(input => input.Code)
            .NotEmpty().WithErrorCode(VehicleTypeExceptionCodes.CodeRequired)
            .MaximumLength(VehicleTypeConsts.MaxCodeLength).WithErrorCode(VehicleTypeExceptionCodes.CodeTooLong);
        RuleFor(input => input.Name)
            .NotEmpty().WithErrorCode(VehicleTypeExceptionCodes.NameRequired)
            .MaximumLength(VehicleTypeConsts.MaxNameLength).WithErrorCode(VehicleTypeExceptionCodes.NameTooLong);
        RuleFor(input => input.SeatingCapacity)
            .GreaterThanOrEqualTo(0).WithErrorCode(VehicleTypeExceptionCodes.SeatingCapacityInvalid);
        RuleFor(input => input.StandingCapacity)
            .GreaterThanOrEqualTo(0).WithErrorCode(VehicleTypeExceptionCodes.StandingCapacityInvalid);
        RuleFor(input => input.WheelchairPlaceCapacity)
            .GreaterThanOrEqualTo(0).WithErrorCode(VehicleTypeExceptionCodes.WheelchairPlaceCapacityInvalid);
        RuleFor(input => input.BicycleCapacity)
            .GreaterThanOrEqualTo(0).WithErrorCode(VehicleTypeExceptionCodes.BicycleCapacityInvalid);
        RuleFor(input => input.Length)
            .GreaterThan(0).WithErrorCode(VehicleTypeExceptionCodes.LengthInvalid)
            .When(input => input.Length.HasValue);
    }
}
