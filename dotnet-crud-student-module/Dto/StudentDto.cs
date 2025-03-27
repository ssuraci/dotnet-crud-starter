using NetCrudStarter.Dto;

namespace NetCrudStarter.StudentModule.Dto;

public class StudentDto: BaseDto<int>
{
    public DateTime? BirthDate { get; set; }

    public string? Category { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    
    public string? Email { get; set; }

}