using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

namespace DAL.Abstracts
{
    public abstract class BasicDalAbstract<T> : IBasicDal<T> where T : class, IEntityUpdatable<T>, IEntity
    {
        /// <summary>
        /// Abstract to get database context
        /// </summary>
        /// <returns></returns>
        protected abstract DbContext GetDbContext();
        
        /// <summary>
        /// Abstract to get entity set
        /// </summary>
        /// <returns></returns>
        protected abstract DbSet<T> GetDbSet();

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter)
        {
            return await GetDbSet().Where(filter).ToListAsync();
        }

        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await GetDbSet().ToListAsync();
        }

        /// <summary>
        /// Returns an entity given the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> Get(int id)
        {
            return (await Get(x => x.Id == id)).FirstOrDefault();
        }

        /// <summary>
        /// Saves an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual async Task<T> Save(T instance)
        {
            GetDbSet().Add(instance);
            await GetDbContext().SaveChangesAsync();
            return instance;
        }

        /// <summary>
        /// Deletes entity given the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> Delete(int id)
        {
            var entity = await Get(id);

            if (entity != null)
            {
                GetDbSet().Remove(entity);
                await GetDbContext().SaveChangesAsync();
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Updates entity given the id and new instance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<T> Update(int id, T dto)
        {
            return await Update(id, entity => entity.Update(dto));
        }

        public async Task<T> Update(int id, Action<T> updater)
        {
            var entity = await Get(id);

            if (entity != null)
            {
                // Copy the fields
                updater(entity);
                GetDbContext().SaveChanges();
                return entity;
            }

            return null;
        }
    }
}