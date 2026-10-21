using FluentValidation;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.Vehicles;

namespace Byd.VehicleManagement.Validators.Vehicles;

// islevi: Vehicle liste isteginin sayfalama sinirlarini dogrular.
// sistemdeki gorevi: Dinamik sorgu repository'sine yalnizca sinirlari belli liste isteklerinin girmesini saglar.
public sealed class VehicleListInputValidator : AbstractValidator<DataTableListInput<VehicleFilter>>
{
    // Sayfa numarasi ve sayfa boyutu kurallarini kaydeder.
    public VehicleListInputValidator()
    {
        RuleFor(input => input.Page)
            .GreaterThanOrEqualTo(DataTablePaging.FirstPage)
            .WithErrorCode(GeneralExceptionCodes.InvalidPage);
        RuleFor(input => input.PageSize)
            .InclusiveBetween(1, DataTablePaging.MaxPageSize)
            .WithErrorCode(GeneralExceptionCodes.InvalidPageSize);
    }
}
