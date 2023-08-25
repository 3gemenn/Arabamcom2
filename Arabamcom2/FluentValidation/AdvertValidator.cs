using Arabamcom2.DTOs;
using FluentValidation;

namespace Arabamcom2.FluentValidation
{
    public class AdvertValidator : AbstractValidator<AdvertDto>
    {
        public AdvertValidator()
        {
            RuleFor(advert => advert.Id).NotEmpty().WithMessage("Advert Id cannot be empty.");
            RuleFor(advert => advert.Title).MaximumLength(500).WithMessage("Title cannot exceed 500 characters.");
            RuleFor(advert => advert.CreatedAt).LessThanOrEqualTo(DateTime.Now).WithMessage("CreatedAt cannot be in the future.");
            RuleFor(advert => advert.CreatedBy).MaximumLength(100).WithMessage("CreatedBy cannot exceed 100 characters.");
            RuleFor(advert => advert.CarId).NotEmpty().WithMessage("CarId cannot be empty.");
            RuleFor(advert => advert.City).MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

        }

    }
}
