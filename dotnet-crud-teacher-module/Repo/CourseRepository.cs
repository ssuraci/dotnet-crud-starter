using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCrudStarter.TeacherModule.Entities;
using NetCrudStarter.Model;
using NetCrudStarter.Repository;
using NetCrudStarter.TeacherModule.Context;
using NetCrudStarter.TeacherModule.Entities.filters;

namespace NetCrudStarter.TeacherModule.Repo;

public class CourseRepository : BaseRepository<Course, int>
{
    protected override Expression<Func<Course, bool>> IdEquals(int id)
    {
        return entity => entity.Id.Equals(id);
    }

    public CourseRepository(ILogger<BaseRepository<Course, int>> logger, TeacherDbContext dbContext) : base(logger, dbContext)
    {
        AddIncludeGraph("WithTeacher", x => x.Teacher);
        AddSortField("title", x => x.Title);
        AddSortField("startDate", x => x.StartDate);
    }

    protected override IQueryable<Course> AddPageModelFilterList(PageModel pageModel, IQueryable<Course> dbQuery)
    {
        foreach (KeyValuePair<string, string> entry in pageModel.FilterDict)
        {
            if (Enum.TryParse(entry.Key, out CourseFilter filter))
            {
                switch (filter)
                {
                    case CourseFilter.IdEq:
                        dbQuery = dbQuery.Where(x => x.Id.Equals(int.Parse(entry.Value)));
                        break;
                    case CourseFilter.TitleLike:
                        dbQuery = dbQuery.Where(x => x.Title!.Contains(entry.Value));
                        break;
                    case CourseFilter.TeacherIdEq:
                        dbQuery = dbQuery.Where(x => x.TeacherId.Equals(int.Parse(entry.Value)));
                        break;
                }
            }
        }
        return dbQuery.OrderBy(x => x.Title);
    }
}
