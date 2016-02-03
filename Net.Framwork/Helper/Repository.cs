﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Net.Framework.Helper
{
    public class Repository<T> : Net.Framework.Helper.IRepository<T> where T : class, new()
    {
        //private static readonly object s_lock = new object();
        private string _constr { get; set; }

        public Repository()
        {
            this._constr = "StoreContext";
        }

        public Repository(string constr)
        {
            this._constr = constr;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                return dbctx.Set<T>().Where(predicate).ToList();
            }
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                return dbctx.Set<T>().FirstOrDefault(predicate);
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                return dbctx.Set<T>().ToList();
            }
        }

        public IEnumerable<T> GetAllOrderBy(Func<T, object> keySelector)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                return dbctx.Set<T>().OrderBy(keySelector).ToList();
            }
        }

        public IEnumerable<T> GetAllOrderByDescending(Func<T, object> keySelector)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                return dbctx.Set<T>().OrderByDescending(keySelector).ToList();
            }
        }

        public int QueryCount(Expression<Func<T, bool>> predicate)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                return dbctx.Set<T>().Count(predicate);
            }
        }

        public bool Insert(T entity)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                dbctx.Set<T>().Add(entity);
                return dbctx.SaveChanges() > 0;
            }
        }

        public IEnumerable<T> InsertAll(List<T> inList)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                var dbSet = dbctx.Set<T>();
                List<T> outList = new List<T>();
                foreach (var item in inList)
                {
                    try
                    {
                        dbctx.Set<T>().Add(item);

                    }
                    catch (Exception)
                    {
                        outList.Add(item);
                        throw;
                    }
                }
                dbctx.SaveChanges();
                return outList;
            }
        }

        public bool Update(T entityToUpdate)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                var dbSet = dbctx.Set<T>();
                var entity = dbSet.Attach(entityToUpdate);
                dbctx.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
                return dbctx.SaveChanges() > 0;
            }
        }

        public IEnumerable<T> UpdateAll(List<T> inList)
        {
            using (var db = new AppDbContext<T>(_constr))
            {
                var dbSet = db.Set<T>();
                List<T> outList = new List<T>();
                foreach (var item in inList)
                {
                    try
                    {
                        var entity = dbSet.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    catch (Exception)
                    {
                        outList.Add(item);
                        throw;
                    }

                }
                db.SaveChanges();
                return outList;
            }
        }

        public bool Delete(object col)
        {
            using (var dbctx = new AppDbContext<T>(_constr))
            {
                T entityToDelete = dbctx.Set<T>().Find(col);
                if (entityToDelete != null)
                {
                    return Delete(entityToDelete);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Delete(Expression<Func<T, bool>> predicate)
        {
            T entityToDelete = First(predicate);
            if (entityToDelete != null)
            {
                return Delete(entityToDelete);
            }
            else
            {
                return false;
            }
        }
    }
}
