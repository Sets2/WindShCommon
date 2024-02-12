using FluentValidation;
using  FluentValidation.AspNetCore;
using WebApi.Models;

namespace WebApi.Validation
{
    public class ValidationModelActivity : AbstractValidator<CreateOrEditActivityRequest>
    {
        public ValidationModelActivity()
        {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("Please specify a Name");
            RuleFor(x => x.IconName)
                   .NotEmpty().WithMessage("Please specify a IconName");
        }
    }
}
