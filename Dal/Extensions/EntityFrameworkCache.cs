using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions
{
    public static class EntityFrameworkCache
    {
        public static TEntity FirstOrDefaultCache<TEntity>(this DbSet<TEntity> queryable, Expression<Func<TEntity, bool>> condition) 
            where TEntity : class
        {
            return queryable.Local.FirstOrDefault(condition.Compile()) // Find in local cache
                   ?? queryable.FirstOrDefault(condition);             // If local cache returns null check the db
        }
    }
}