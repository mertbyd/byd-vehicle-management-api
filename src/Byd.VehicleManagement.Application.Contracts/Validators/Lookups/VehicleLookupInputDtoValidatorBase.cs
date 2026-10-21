using FluentValidation;
using Byd.VehicleManagement.Dtos.Lookups;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.Lookups;

// islevi: Lookup olusturma ve guncelleme isteklerinin ortak alan kurallarini tanimlar.
// sistemdeki gorevi: Ad zorunlulugu ve kod araligini tek dogrulayici tabaninda toplar.
public abstract class VehicleLookupInputDtoValidatorBase<TDto> : AbstractValidator<TDto>
    where TDto : VehicleLookupInputDto
{
    protected VehicleLookupInputDtoValidatorBase()
    {
        RuleFor(input => input.Name)
            .NotEmpty().WithErrorCode(VehicleLookupExceptionCodes.NameRequired);
        RuleFor(input => input.Code)
            .GreaterThanOrEqualTo(0).WithErrorCode(VehicleLookupExceptionCodes.CodeInvalid);
    }
}
