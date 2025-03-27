using Microsoft.Extensions.Logging;
using NetCrudStarter.TeacherModule.Entities;
using NetCrudStarter.TeacherModule.Repo;
using NetCrudStarter.Middleware;
using NetCrudStarter.Repository;
using NetCrudStarter.Service;

namespace NetCrudStarter.TeacherModule.Service;

public class TeacherService: BaseService<Teacher, int>
{
    public TeacherService(ILogger<TeacherService> logger, TeacherRepository repository): base(logger, repository)
    {
    }

}