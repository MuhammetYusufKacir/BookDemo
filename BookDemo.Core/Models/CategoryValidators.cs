using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using FluentValidation;

namespace BookDemo.Core.Models
{
    public class CategoryValidators : AbstractValidator<CategoryDTO>
    {
        public CategoryValidators() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
        }
    }
}
