using NetCrudStarter.Dto;

namespace NetCrudStarter.Demo.Dto;

public class TeacherDto: BaseDto<int>
{
    public DateOnly? BirthDate { get; set; }

    public string? Category { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? SchoolId { get; set; }

    public int? SubjectId { get; set; }

    public string? Email { get; set; }

    public string? SchoolName { get; set; }
}