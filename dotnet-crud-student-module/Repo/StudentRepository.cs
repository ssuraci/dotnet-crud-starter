using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCrudStarter.StudentModule.Entities;
using NetCrudStarter.StudentModule.Entities.filters;
using NetCrudStarter.Model;
using NetCrudStarter.Repository;
using NetCrudStarter.StudentModule.Context;

namespace NetCrudStarter.StudentModule.Repo;

public class StudentRepository: BaseRepository<Student, int>
{
    protected override Expression<Func<Student, bool>> IdEquals(int id)
    {
        return entity => entity.Id.Equals(id);
    }

    public StudentRepository(ILogger<BaseRepository<Student, int>> logger, StudentDbContext dbContext) : base(logger, dbContext)
    {
        AddSortField("lastName", x => x.LastName);
        AddSortField("birthDate", x => x.BirthDate);
    }
    
    
    
    protected override IQueryable<Student> AddPageModelFilterList(PageModel pageModel, IQueryable<Student> dbQuery)
    {
        foreach (KeyValuePair<string, string> entry in pageModel.FilterDict)
        {
            if (Enum.TryParse(entry.Key, out StudentFilter filter))
            {
                switch (filter)
                {
                    case StudentFilter.StrLastNameLike:
                        dbQuery = dbQuery.Where(x => x.LastName!.StartsWith(entry.Value));
                        break;
                    case StudentFilter.CourseEnroledIdEq:
                        Int32 enrolledCourseId = Convert.ToInt32(entry.Value);
                        dbQuery = dbQuery.Where(x => x.Enrolments.Any(e => e.CourseId == enrolledCourseId));
                        break;
                }
                
            }
        }
        return dbQuery.OrderBy(x => x.LastName);
    }
}
