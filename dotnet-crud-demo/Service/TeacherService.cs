using NetCrudLib.Demo.Entities;
using NetCrudLib.Demo.Repo;
using NetCrudLib.Middleware;
using NetCrudLib.Repository;
using NetCrudLib.Service;

namespace NetCrudLib.Demo.Service;

public class TeacherService: BaseService<Teacher, int>
{
    public TeacherService(ILogger<TeacherService> logger, TeacherRepository repository): base(logger, repository)
    {
    }

}