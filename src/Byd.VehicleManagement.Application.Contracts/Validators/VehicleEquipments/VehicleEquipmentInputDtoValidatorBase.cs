using FluentValidation;
using Byd.VehicleManagement.Consts;
using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.VehicleEquipments;

// islevi: Arac donanimi tur ve adet kurallarini tek yerde dogrular.
// sistemdeki gorevi: Olusturma ve guncelleme isteklerinin ayni sekil sozlesmesinden gecmesini saglar; baglarin var olup
// olmadigi ve tur tekilligi Manager'da kontrol edilir.
public abstract class VehicleEquipmentInputDtoValidatorBase<TDto> : AbstractValidator<TDto>
    where TDto : VehicleEquipmentInputDto
{
    // Donanim turu secimi ve adet alt siniri kurallarini kaydeder.
    protected VehicleEquipmentInputDtoValidatorBase()
    {
        RuleFor(input => input.EquipmentTypeId)
            .GreaterThan(0).WithErrorCode(VehicleEquipmentExceptionCodes.EquipmentTypeRequired);
        RuleFor(input => input.Units)
            .GreaterThanOrEqualTo(VehicleEquipmentConsts.MinUnits).WithErrorCode(VehicleEquipmentExceptionCodes.UnitsInvalid);
    }
}
