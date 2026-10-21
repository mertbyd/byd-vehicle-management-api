using FluentValidation;
using Byd.CoreExtensions.ExceptionCodes;
using Byd.CoreExtensions.Querying;
using Byd.VehicleManagement.Dtos.VehicleModels;

namespace Byd.VehicleManagement.Validators.VehicleModels;

// islevi: VehicleModel liste isteginin sayfalama sinirlarini dogrular.
// sistemdeki gorevi: Dinamik sorgu repository'sine yalnizca sinirlari belli liste isteklerinin girmesini saglar.
public sealed class VehicleModelListInputValidator : AbstractValidator<DataTableListInput<VehicleModelFilter>>
{
    // Sayfa numarasi ve sayfa boyutu kurallarini kaydeder.
    public VehicleModelListInputValidator()
    {
        RuleFor(input => input.Page)
            .GreaterThanOrEqualTo(DataTablePaging.FirstPage)
            .WithErrorCode(GeneralExceptionCodes.InvalidPage);
        RuleFor(input => input.PageSize)
            .InclusiveBetween(1, DataTablePaging.MaxPageSize)
            .WithErrorCode(GeneralExceptionCodes.InvalidPageSize);
    }
}
