using Net.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Net.Framework
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        protected readonly StoreContext dbcontext;
        private readonly IDbSet<T> dbset;

        public Repository()
        {
            dbcontext = new StoreContext();
            dbset = dbcontext.Set<T>();
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return dbcontext.Set<T>().Where(predicate).ToList();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return dbcontext.Set<T>().Where(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return dbcontext.Set<T>().ToList();
        }

        public IEnumerable<T> GetAllOrderBy(Func<T, object> keySelector)
        {
            return dbcontext.Set<T>().OrderBy(keySelector).ToList();
        }

        public IEnumerable<T> GetAllOrderByDescending(Func<T, object> keySelector)
        {
            return dbcontext.Set<T>().OrderByDescending(keySelector).ToList();
        }

        public void Add(T entity)
        {
            dbcontext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            dbset.Attach(entity);
            dbcontext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            dbcontext.Set<T>().Remove(entity);
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}