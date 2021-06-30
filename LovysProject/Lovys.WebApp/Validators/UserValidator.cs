using FluentValidation;
using Lovys.Core.Calendar.Entities;

namespace Lovys.WebApp.Validators
{
    public class UserValidator : AbstractValidator<User> 
    {
        public UserValidator() {
            RuleFor(x => x.Id).Null();
            RuleFor(x => x.Name).NotEmpty().NotNull().Length(1, 30);
            RuleFor(x => x.Role).NotNull().IsInEnum();
        }
    }
}