using Core.Calendar.Models;
using FluentValidation;

namespace WebApp.Validators
{
    public class UserValidator : AbstractValidator<User> 
    {
        public UserValidator() {
            RuleFor(x => x.Name).NotEmpty().NotNull().Length(1, 30);
            RuleFor(x => x.Role).NotNull().IsInEnum();
        }
    }
}