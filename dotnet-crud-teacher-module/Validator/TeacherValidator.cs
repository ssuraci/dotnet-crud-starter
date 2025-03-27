using FluentValidation;
using NetCrudStarter.TeacherModule.Dto;

namespace NetCrudStarter.TeacherModule.Validator;

public class TeacherValidator: AbstractValidator<TeacherDto>
{
    public TeacherValidator()
    {
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
    }
}
