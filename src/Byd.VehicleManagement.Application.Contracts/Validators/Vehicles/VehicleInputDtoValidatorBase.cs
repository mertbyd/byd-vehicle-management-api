using FluentValidation;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.Vehicles;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.Vehicles;

// islevi: Arac alanlarinin zorunluluk, uzunluk ve hizmet donemi kurallarini tek yerde dogrular.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ayni sekil sozlesmesinden gecmesini saglar; baglarin var olup
// olmadigi ve tekillikler Manager'da kontrol edilir.
public abstract class VehicleInputDtoValidatorBase<TDto> : AbstractValidator<TDto>
    where TDto : VehicleInputDto
{
    // Bag secimi, kimlik ve metin uzunluklari ve tarih sirasi kurallarini kaydeder.
    protected VehicleInputDtoValidatorBase()
    {
        RuleFor(input => input.TransportModeId)
            .GreaterThan(0).WithErrorCode(VehicleExceptionCodes.TransportModeRequired);
        RuleFor(input => input.VehicleStatusId)
            .GreaterThan(0).WithErrorCode(VehicleExceptionCodes.VehicleStatusRequired);
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
        RuleFor(input => input.InServiceFrom)
            .NotEmpty().WithErrorCode(VehicleExceptionCodes.InServiceFromRequired);
        RuleFor(input => input.InServiceUntil)
            .Must((input, until) => until >= input.InServiceFrom)
            .WithErrorCode(VehicleExceptionCodes.InServiceUntilBeforeFrom)
            .When(input => input.InServiceUntil.HasValue);
    }
}
