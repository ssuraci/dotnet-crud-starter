using NetCrudStarter.Demo.Entities;
using NetCrudStarter.Demo.Repo;
using NetCrudStarter.Middleware;
using NetCrudStarter.Repository;
using NetCrudStarter.Service;

namespace NetCrudStarter.Demo.Service;

public class TeacherService: BaseService<Teacher, int>
{
    public TeacherService(ILogger<TeacherService> logger, TeacherRepository repository): base(logger, repository)
    {
    }

}