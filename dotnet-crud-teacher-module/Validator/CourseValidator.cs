using FluentValidation;
using NetCrudStarter.TeacherModule.Dto;

namespace NetCrudStarter.TeacherModule.Validator;

public class CourseValidator: AbstractValidator<CourseDto>
{
    public CourseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
    }
}
