using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Net.Framework
{
    public class AppDbContext<T> : DbContext where T : class
    {
        static AppDbContext()
        {
            Database.SetInitializer<AppDbContext<T>>(null);
        }

        public AppDbContext(string constr)
            : base(constr)
        {
        }

        public DbSet<T> dbSet { get; set; }
    }
}
