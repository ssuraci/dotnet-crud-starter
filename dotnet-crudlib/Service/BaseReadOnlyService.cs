using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NetCrudLib.Model;
using NetCrudLib.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using NetCrudLib.Middleware;

namespace NetCrudLib.Service
{
    public abstract class BaseReadOnlyService<T, TK>(ILogger<BaseReadOnlyService<T, TK>> logger, BaseReadOnlyRepository<T, TK> repository)  where T : BaseEntity<TK>
    {
        protected readonly ILogger Logger = logger;
        
        protected readonly  BaseReadOnlyRepository<T, TK> Repository = repository;



        protected virtual void BeforeGetByPageModel(PageModel pageModel)
        {

        }

        protected virtual PagedResult<T> AfterGetByPageModel(PageModel pageModel, PagedResult<T> itemList)
        {
            return itemList;
        }


        public virtual async Task<IList<T>> GetAll()
        {
            return await Repository.GetAll();
        }

        public virtual async Task<T?> GetById(TK id, PageModel pageModel = null)
        {
            return await Repository.GetById(id, pageModel);
        }

        public virtual async Task<IList<T>> GetList(Expression<Func<T, bool>> where, string includeGraph)
        {
            return await Repository.GetList(where, includeGraph);
        }

        public virtual async Task<PagedResult<T>> GetByPageModel(PageModel pageModel)
        {
            BeforeGetByPageModel(pageModel);
            var res = await Repository.GetByPageModel(pageModel);
            return AfterGetByPageModel(pageModel, res);
        }
    }
}