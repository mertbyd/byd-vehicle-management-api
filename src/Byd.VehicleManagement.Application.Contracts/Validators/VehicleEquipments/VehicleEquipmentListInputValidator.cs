using FluentValidation;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleEquipments;

namespace Byd.VehicleManagement.Validators.VehicleEquipments;

// islevi: VehicleEquipment liste isteginin sayfalama sinirlarini dogrular.
// sistemdeki gorevi: Dinamik sorgu repository'sine yalnizca sinirlari belli liste isteklerinin girmesini saglar.
public sealed class VehicleEquipmentListInputValidator : AbstractValidator<DataTableListInput<VehicleEquipmentFilter>>
{
    // Sayfa numarasi ve sayfa boyutu kurallarini kaydeder.
    public VehicleEquipmentListInputValidator()
    {
        RuleFor(input => input.Page)
            .GreaterThanOrEqualTo(DataTablePaging.FirstPage)
            .WithErrorCode(GeneralExceptionCodes.InvalidPage);
        RuleFor(input => input.PageSize)
            .InclusiveBetween(1, DataTablePaging.MaxPageSize)
            .WithErrorCode(GeneralExceptionCodes.InvalidPageSize);
    }
}
