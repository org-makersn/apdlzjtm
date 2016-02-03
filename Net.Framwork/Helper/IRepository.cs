using System;

namespace Net.Framwork.Helper
{
    interface IRepository<T>
     where T : class, new()
    {
        bool Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        bool Delete(object col);
        T First(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        System.Collections.Generic.IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        System.Collections.Generic.IEnumerable<T> GetAll();
        System.Collections.Generic.IEnumerable<T> GetAllOrderBy(Func<T, object> keySelector);
        System.Collections.Generic.IEnumerable<T> GetAllOrderByDescending(Func<T, object> keySelector);
        int Insert(T entity);
        System.Collections.Generic.IEnumerable<T> InsertAll(System.Collections.Generic.List<T> inList);
        int QueryCount(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        bool Update(T entityToUpdate);
        System.Collections.Generic.IEnumerable<T> UpdateAll(System.Collections.Generic.List<T> inList);
    }
}
