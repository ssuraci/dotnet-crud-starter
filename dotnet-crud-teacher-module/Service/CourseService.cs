using Microsoft.Extensions.Logging;
using NetCrudStarter.TeacherModule.Entities;
using NetCrudStarter.TeacherModule.Repo;
using NetCrudStarter.Service;

namespace NetCrudStarter.TeacherModule.Service;

public class CourseService : BaseService<Course, int>
{
    public CourseService(ILogger<CourseService> logger, CourseRepository repository) : base(logger, repository)
    {
    }
}
