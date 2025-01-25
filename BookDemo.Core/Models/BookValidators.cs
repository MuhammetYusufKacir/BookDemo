using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using FluentValidation;

namespace BookDemo.Core.Models
{
    public class BookValidators : AbstractValidator<BookDTO>
    {
        public BookValidators()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId cannot be empty.");
        }
    }
}
