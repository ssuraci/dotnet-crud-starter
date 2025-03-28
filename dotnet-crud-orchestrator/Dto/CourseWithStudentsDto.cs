using NetCrudStarter.StudentModule.Dto;
using NetCrudStarter.TeacherModule.Dto;

namespace NetCrudStarter.Orchestrator.Dto
{
    public class CourseWithStudentsDto
    {
        public CourseDto Course { get; set; }
        public List<StudentDto> Students { get; set; }
    }
}
