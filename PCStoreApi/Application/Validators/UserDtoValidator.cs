using FluentValidation;
using PCStoreApi.Application.DTOs.Auth;

namespace PCStoreApi.Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain one number.")
                .Matches("[^a-zA-z0-9]").WithMessage("Password must contain one special character.");
        }
    }
}
