using FluentValidation;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleTypes;

namespace Byd.VehicleManagement.Validators.VehicleTypes;

// islevi: VehicleType liste isteginin sayfalama sinirlarini dogrular.
// sistemdeki gorevi: Dinamik sorgu repository'sine yalnizca sinirlari belli liste isteklerinin girmesini saglar.
public sealed class VehicleTypeListInputValidator : AbstractValidator<DataTableListInput<VehicleTypeFilter>>
{
    // Sayfa numarasi ve sayfa boyutu kurallarini kaydeder.
    public VehicleTypeListInputValidator()
    {
        RuleFor(input => input.Page)
            .GreaterThanOrEqualTo(DataTablePaging.FirstPage)
            .WithErrorCode(GeneralExceptionCodes.InvalidPage);
        RuleFor(input => input.PageSize)
            .InclusiveBetween(1, DataTablePaging.MaxPageSize)
            .WithErrorCode(GeneralExceptionCodes.InvalidPageSize);
    }
}
