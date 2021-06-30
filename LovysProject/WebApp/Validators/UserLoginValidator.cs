using System.Reflection.Metadata.Ecma335;
using Core.Calendar.Models;
using FluentValidation;

namespace WebApp.Validators
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