using Arabamcom2.DTOs;
using FluentValidation;

namespace Arabamcom2.FluentValidation
{
    public class IdValidator : AbstractValidator<IdDto>
    {
        public IdValidator()
        {
            RuleFor(advert => advert.Id).NotEmpty().WithMessage("Advert Id cannot be empty.").NotNull().WithMessage("Id is Required");
        }
    }
}
