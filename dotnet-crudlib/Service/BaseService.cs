using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NetCrudStarter.Model;
using NetCrudStarter.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using NetCrudStarter.Middleware;

namespace NetCrudStarter.Service
{
    public abstract class BaseService<T, TK>(ILogger<BaseService<T, TK>> logger, BaseRepository<T, TK> repository): BaseReadOnlyService<T, TK>(logger, repository) where T : BaseEntity<TK>
    {
        protected new readonly  BaseRepository<T, TK> Repository = repository;
        
        [TransactionAspect]
        public virtual async Task<T> Add(T entity)
        {
            return await Repository.Add(entity);
        }

        [TransactionAspect]
        public virtual async Task Save(List<T> entityList)
        {
           await Repository.Save(entityList);
        }

        [TransactionAspect]
        public virtual async Task<T> Update(T entity)
        {
            return await Repository.Update(entity);
        }
        
        [TransactionAspect]
        public virtual async Task Remove(T entity)
        {
            await Repository.Remove(entity);
        }

        [TransactionAspect]
        public virtual async Task RemoveById(TK id)
        {
            await Repository.RemoveById(id);
        }
        
        [TransactionAspect]
        public virtual async Task Remove(List<T> itemList)
        {
            await Repository.Remove(itemList);
        }
        
    }
}