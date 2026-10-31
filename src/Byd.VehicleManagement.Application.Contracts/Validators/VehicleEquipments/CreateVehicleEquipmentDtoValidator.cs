using FluentValidation;
using Byd.VehicleManagement.Dtos.VehicleEquipments;
using Byd.VehicleManagement.ExceptionCodes;

namespace Byd.VehicleManagement.Validators.VehicleEquipments;

// mirastan gelen: Donanim turu ve adet kurallari.
// burada yazilan: Arac seciminin zorunlulugu.
public sealed class CreateVehicleEquipmentDtoValidator : VehicleEquipmentInputDtoValidatorBase<CreateVehicleEquipmentDto>
{
    // Arac kimliginin bos gelmemesini kaydeder.
    public CreateVehicleEquipmentDtoValidator()
    {
        RuleFor(input => input.VehicleId)
            .NotEmpty().WithErrorCode(VehicleEquipmentExceptionCodes.VehicleRequired);
    }
}
