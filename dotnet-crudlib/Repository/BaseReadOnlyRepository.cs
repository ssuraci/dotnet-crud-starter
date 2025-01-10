using System.Linq.Expressions;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using NetCrudLib.Model;

namespace NetCrudLib.Repository;

public class BaseReadOnlyRepository<T, TK>(ILogger<BaseReadOnlyRepository<T, TK>> logger, DbContext dbContext) where T : BaseEntity<TK>
{
    protected readonly ILogger Logger = logger;

    protected ExpressionMap<T, TK> IncludeGraphMap = new();

    protected ExpressionMap<T, TK> SortMap = new();

    protected DbContext Context = dbContext;
    
            public void AddIncludeGraph(string key, params Expression<Func<T, object>>[] value)
            {
                IncludeGraphMap.Add(key, value);
            }
            public void AddSortField(string key, Expression<Func<T, object>> value)
            {
                SortMap.Add(key, value);
            }
    
            protected virtual Expression<Func<T, bool>> IdEquals(TK id)
            {
                return entity => !EqualityComparer<TK>.Default.Equals(entity.Id, default(TK)) && entity.Id.Equals(id);
            }
    
            protected virtual IQueryable<T> GetDbQuery(IQueryable<T>? dbQuery = null)
            {
                dbQuery ??= Context.Set<T>();
                return Transaction.Current != null ? dbQuery : dbQuery.AsNoTracking();
            }
    
            protected virtual Expression<Func<T, TK>> DefaultOrderBy()
            {
                return entity => entity.Id!;
            }
            public virtual async Task<List<T>> GetAll(string? includeGraph = null)
            {
                var dbQuery = AddIncludeGraph(includeGraph, GetDbQuery());
                return await dbQuery.ToListAsync<T>();
            }
    
    
            public virtual async Task<List<T>> GetList(Expression<Func<T, bool>> where, string? includeGraph = null)
            {
                var dbQuery = AddIncludeGraph(includeGraph, GetDbQuery());
                return await dbQuery.Where(where).ToListAsync<T>();
            }
    
    
            public virtual async Task<T?> GetSingle(Expression<Func<T, bool>> where, string? includeGraph = null)
            {
                var dbQuery = AddIncludeGraph(includeGraph, GetDbQuery());
                return await dbQuery.FirstOrDefaultAsync(where); //Apply where clause
            }
    
            public virtual async Task<T?> GetById(TK id, PageModel? pageModel = null)
            {
                IQueryable<T> dbQuery = GetDbQuery();
                dbQuery = dbQuery.Where(IdEquals(id));
                if (pageModel != null)
                {
                    dbQuery = AddIncludeGraph(pageModel.IncludeGraph, dbQuery);
                }
                return await dbQuery.FirstOrDefaultAsync(); //Apply where clause
            }
            public virtual async Task<List<T>> ReturnPaged(IQueryable<T> dbQuery, PageModel pageModel)
            {
                List<T> list = pageModel.PageItems > 0
                    ? await dbQuery.Skip((pageModel.PageStart - 1) * pageModel.PageItems).Take(pageModel.PageItems)
                        .ToListAsync()
                    : await dbQuery.ToListAsync();
                return list;
            }

            protected virtual IQueryable<T> AddPageModelFilterList(PageModel pageModel, IQueryable<T> dbQuery)
            {
                return dbQuery;
            }

            protected virtual IQueryable<T> AddIncludeGraph(string? includeGraph, IQueryable<T> dbQuery)
            {
                if (includeGraph != null && IncludeGraphMap.Items.TryGetValue(includeGraph,
                        out Expression<Func<T, object>>[]? navigationProperties))
                {
                    navigationProperties.ToList().ForEach(np => dbQuery = dbQuery.Include<T, object>(np));
                }
                else
                {
                    Logger.LogWarning("IncludeGraph not found");
                }

                return dbQuery;
            }

            protected virtual IQueryable<T> AddSort(PageModel pageModel, IQueryable<T> dbQuery)
            {
                bool customSort = false;
                foreach (var sort in pageModel.SortList)
                {
                    var sortDesc = sort.StartsWith("-");
                    var sortField = sort[1..];
                    if (SortMap.Items.TryGetValue(sortField, out Expression<Func<T, object>>[]? sortFields))
                    {
                        dbQuery = sortDesc ? dbQuery.OrderByDescending(sortFields[0])
                            : dbQuery.OrderBy(sortFields[0]);
                        customSort = true;
                    }
                }

                return customSort ? dbQuery : dbQuery.OrderBy(DefaultOrderBy());
            }


            public virtual async Task<PagedResult<T>> GetByPageModel(PageModel pageModel)
            {
                PagedResult<T> result = new();
                IQueryable<T> dbQuery = GetDbQuery();

                dbQuery = AddPageModelFilterList(pageModel, dbQuery);
                result.TotalItems = await dbQuery.CountAsync();

                dbQuery = AddIncludeGraph(pageModel.IncludeGraph, dbQuery);
                dbQuery = AddSort(pageModel, dbQuery);
                result.Items = await ReturnPaged(dbQuery, pageModel);
                return result;
            }

}