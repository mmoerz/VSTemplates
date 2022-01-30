using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class $ModelName$Validator : AbstractValidator<$ModelName$Model>
    {
        public $ModelName$Validator()
        {
            // RuleFor(p => p.Example).NotEmpty().WithMessage("Stadt muss angegeben werden.");
        }
    }
}
