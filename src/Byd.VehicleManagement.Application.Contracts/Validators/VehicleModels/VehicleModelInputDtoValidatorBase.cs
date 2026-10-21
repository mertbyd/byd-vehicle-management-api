using FluentValidation;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.VehicleModels;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.VehicleModels;

// islevi: Arac modeli alanlarinin zorunluluk ve uzunluk kurallarini tek yerde dogrular.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ayni sekil sozlesmesinden gecmesini saglar.
public abstract class VehicleModelInputDtoValidatorBase<TDto> : AbstractValidator<TDto>
    where TDto : VehicleModelInputDto
{
    // Uretici ve model adi kurallarini kaydeder.
    protected VehicleModelInputDtoValidatorBase()
    {
        RuleFor(input => input.Manufacturer)
            .NotEmpty().WithErrorCode(VehicleModelExceptionCodes.ManufacturerRequired)
            .MaximumLength(VehicleModelConsts.MaxManufacturerLength)
            .WithErrorCode(VehicleModelExceptionCodes.ManufacturerTooLong);
        RuleFor(input => input.Name)
            .NotEmpty().WithErrorCode(VehicleModelExceptionCodes.NameRequired)
            .MaximumLength(VehicleModelConsts.MaxNameLength).WithErrorCode(VehicleModelExceptionCodes.NameTooLong);
    }
}
