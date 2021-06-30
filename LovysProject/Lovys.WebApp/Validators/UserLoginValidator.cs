using FluentValidation;
using Lovys.Core.Calendar.Entities;

namespace Lovys.WebApp.Validators
{
    public class UserLoginValidator : AbstractValidator<User>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}