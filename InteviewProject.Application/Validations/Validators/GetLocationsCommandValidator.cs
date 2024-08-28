using FluentValidation;
using InteviewProject.Application.Validations.Commands;

namespace InteviewProject.Application.Validations.Validators
{
    public class GetLocationsCommandValidator : AbstractValidator<GetLocationsCommand>
    {
        public GetLocationsCommandValidator()
        {
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .Length(3, 50).WithMessage("Location must be between 3 and 50 characters.")
                .Matches(@"^[\w]+$").WithMessage("Selected Key Location must not contain spaces or punctuation.");
        }
    }
}
