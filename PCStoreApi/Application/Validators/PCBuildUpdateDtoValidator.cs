using FluentValidation;
using PCStoreApi.Application.DTOs.PCBuild;

namespace PCStoreApi.Application.Validators
{
    public class PCBuildUpdateDtoValidator : AbstractValidator<PCBuildUpdateDto>
    {
        public PCBuildUpdateDtoValidator()
        {
            RuleFor(x => x.Processor).NotEmpty().WithMessage("Processor is required.");
            RuleFor(x => x.RamInGB).GreaterThan(0).WithMessage("RAM must be greater than 0.");
            RuleFor(x => x.Storage).NotEmpty().WithMessage("Storage is required.");
        }
    }
}
