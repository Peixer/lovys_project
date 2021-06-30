using Core.Calendar.Models;
using FluentValidation;

namespace WebApp.Validators
{
    public class AvailabilityValidator : AbstractValidator<Availability> 
    {
        public AvailabilityValidator() {
            RuleFor(x => x.Id).Null();
            RuleFor(x => x.EndTime).NotEmpty().NotNull().Matches("(1[012]|0[1-9])([Aa]|[pP])[mM]");
            RuleFor(x => x.StartTime).NotEmpty()
                .NotNull()
                .Matches("(1[012]|0[1-9])([Aa]|[pP])[mM]")
                .LessThan(x=>x.EndTime);

            RuleFor(x => x.DayOfWeek).NotNull().IsInEnum();
        }
    }
}