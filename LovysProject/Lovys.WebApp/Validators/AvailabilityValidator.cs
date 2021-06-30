using FluentValidation;
using Lovys.Core.Calendar.Entities;

namespace Lovys.WebApp.Validators
{
    public class AvailabilityValidator : AbstractValidator<Availability> 
    {
        public AvailabilityValidator() {
            RuleFor(x => x.Id).Null();
            RuleFor(x => x.EndTime).NotEmpty().NotNull().Matches("([0-1]?[0-9])(am|pm)");
            RuleFor(x => x.StartTime).NotEmpty()
                .NotNull()
                .Matches("([0-1]?[0-9])(am|pm)");

            RuleFor(x => x.DayOfWeek).NotNull().IsInEnum();
        }
    }
}