using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCrudStarter.Demo.Entities;
using NetCrudStarter.Demo.Entities.filters;
using NetCrudStarter.Model;
using NetCrudStarter.Repository;

namespace NetCrudStarter.Demo.Repo;

public class TeacherRepository: BaseRepository<Teacher, int>
{
    protected override Expression<Func<Teacher, bool>> IdEquals(int id)
    {
        return entity => entity.Id.Equals(id);
    }

    public TeacherRepository(ILogger<BaseRepository<Teacher, int>> logger, DbContext dbContext) : base(logger, dbContext)
    {
        AddIncludeGraph("WithSchool",  x => x.School);
        AddIncludeGraph("WithSchoolAndCourses", [ x => x.School, x => x.Courses]);
        AddSortField("lastName", x => x.LastName);
        AddSortField("birthDate", x => x.BirthDate);
    }
    
    
    
    protected override IQueryable<Teacher> AddPageModelFilterList(PageModel pageModel, IQueryable<Teacher> dbQuery)
    {
        foreach (KeyValuePair<string, string> entry in pageModel.FilterDict)
        {
            if (Enum.TryParse(entry.Key, out TeacherFilter filter))
            {
                switch (filter)
                {
                    case TeacherFilter.StrLastNameLike:
                        dbQuery = dbQuery.Where(x => x.LastName!.StartsWith(entry.Value));
                        break;
                    case TeacherFilter.SchoolIdEq:
                        dbQuery = dbQuery.Where(x => x.School != null && x.School.Id.Equals(entry.Value));
                        break;
                }
                
            }
        }
        return dbQuery.OrderBy(x => x.LastName);
    }
}