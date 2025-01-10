using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NetCrudStarter.Model;
using System.Transactions;

namespace NetCrudStarter.Repository
{
    public class BaseRepository<T, TK>(ILogger<BaseRepository<T, TK>> logger, DbContext dbContext)
        : BaseReadOnlyRepository<T, TK>(logger, dbContext) where T : BaseEntity<TK>
    {



        protected virtual async Task<T> ReturnItem(T item)
        {
            IQueryable<T> dbQuery = GetDbQuery();
            if (item == null || EqualityComparer<TK>.Default.Equals(item.Id, default(TK)))
            {
                throw new ArgumentNullException(nameof(item), "Item or Item.Id cannot be null");
            }

            return await dbQuery.Where(IdEquals(item.Id)).FirstOrDefaultAsync() ??
                   throw new InvalidOperationException("Item not found");
        }

        public virtual async Task<T> Add(T item)
        {
            Context.Entry(item).State = EntityState.Added;
            await Context.SaveChangesAsync();
            return await ReturnItem(item);

        }

        public virtual async Task Add(List<T> items)
        {
            items.ForEach(i => Context.Entry(i).State = EntityState.Added);
            await Context.SaveChangesAsync();
        }


        public virtual async Task Save(List<T> items)
        {
            foreach (T item in items)
            {
                if (EqualityComparer<TK>.Default.Equals(item.Id, default(TK)))
                {
                    await Add(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        public virtual async Task Update(List<T> items)
        {
            items.ForEach(i => Context.Entry(i).State = EntityState.Modified);
            await Context.SaveChangesAsync();
        }


        public virtual async Task<T> Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return await ReturnItem(item);
        }

        public virtual async Task Remove(List<T> items)
        {
            items.ForEach(i => Context.Entry(i).State = EntityState.Deleted);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Remove(T item)
        {
            await Remove([item]);
        }

        public virtual async Task RemoveById(TK id)
        {
            T? res = await GetById(id);
            if (res != null)
            {
                Context.Remove(res);
                await Context.SaveChangesAsync();
            }
        }





        public void DetachAllEntries()
        {
            foreach (var entry in Context.ChangeTracker.Entries().ToList())
            {
                Context.Entry(entry.Entity).State = EntityState.Detached;
            }
        }


     

    }
}
