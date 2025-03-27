using Microsoft.Extensions.Logging;
using NetCrudStarter.StudentModule.Entities;
using NetCrudStarter.StudentModule.Repo;
using NetCrudStarter.Middleware;
using NetCrudStarter.Repository;
using NetCrudStarter.Service;

namespace NetCrudStarter.StudentModule.Service;

public class StudentService: BaseService<Student, int>
{
    public StudentService(ILogger<StudentService> logger, StudentRepository repository): base(logger, repository)
    {
    }

}