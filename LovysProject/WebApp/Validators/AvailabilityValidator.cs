using Core.Calendar.Models;
using FluentValidation;

namespace WebApp.Validators
{
    public class AvailabilityValidator : AbstractValidator<Availability> 
    {
        public AvailabilityValidator() {
            RuleFor(x => x.Id).Null();
            RuleFor(x => x.EndTime).NotEmpty().NotNull();
            RuleFor(x => x.StartTime).NotEmpty().NotNull();
            RuleFor(x => x.DayOfWeek).NotNull().IsInEnum();
        }
    }
}