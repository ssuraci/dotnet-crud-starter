using NetCrudStarter.Dto;

namespace NetCrudStarter.TeacherModule.Dto;

public class CourseDto: BaseDto<int>
{
    public string? Description { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? StartDate { get; set; }

    public string? Title { get; set; }

    public int? TeacherId { get; set; }
    
    public string? TeacherName { get; set; }
}
