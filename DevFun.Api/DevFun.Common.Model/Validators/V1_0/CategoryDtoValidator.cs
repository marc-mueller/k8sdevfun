using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevFun.Common.Model.Dtos.V1_0;
using FluentValidation;

namespace DevFun.Common.Model.Validators.V1_0
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
