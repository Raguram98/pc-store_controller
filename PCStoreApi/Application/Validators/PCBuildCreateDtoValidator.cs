using FluentValidation;
using PCStoreApi.Application.DTOs.PCBuild;

namespace PCStoreApi.Application.Validators
{
    public class PCBuildCreateDtoValidator : AbstractValidator<PCBuildCreateDto>
    {
        public PCBuildCreateDtoValidator()
        {
            Console.WriteLine("Ommala okkadika poranda");
            RuleFor(x => x.Processor).NotEmpty().WithMessage("Processor is required.");
            RuleFor(x => x.RamInGB).GreaterThan(0).WithMessage("RAM must be greater than 0.");
            RuleFor(x => x.Storage).NotEmpty().WithMessage("Storage is required.");
            RuleFor(x => x.UserID).GreaterThan(0).WithMessage("UserID must be greater than 0.");
        }
    }
}
