using FluentValidation;
using InteviewProject.Application.Validations.Commands;

namespace InteviewProject.Application.Validations.Validators
{
    public class GetForecastsCommandValidator : AbstractValidator<GetForecastsCommand>
    {
        public GetForecastsCommandValidator()
        {
            RuleFor(x => x.SelectedKeyLocation)
                .NotEmpty().WithMessage("Selected Key Location is required.")
                .Matches(@"^[\w]+$").WithMessage("Selected Key Location must not contain spaces or punctuation.");
        }
    }
}
