using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileObjects.AgileMapper;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

namespace Dal.Abstracts
{
    public abstract class BasicDalRelationalAbstract<T> : IBasicDal<T> where T : class, IEntity
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

        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await Intercept(GetDbSet()).ToListAsync();
        }

        /// <summary>
        /// Returns an entity given the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> Get(int id)
        {
            return await Intercept(GetDbSet()).FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Saves an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual async Task<T> Save(T instance)
        {
            await GetDbSet().AddAsync(instance);
            
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
            var entity = await Get(id);

            if (entity != null)
            {
                var result = UpdateEntity(entity, dto);
                
                await GetDbContext().SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task<T> Update(int id, Action<T> updater)
        {
            var entity = await Get(id);

            if (entity != null)
            {
                updater(entity);
                
                await GetDbContext().SaveChangesAsync();
                
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Intercept the IQueryable to include
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<T> Intercept<TQueryable>(TQueryable queryable) where TQueryable : IQueryable<T>
        {
            return queryable;
        }

        /// <summary>
        /// Default update method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual T UpdateEntity(T entity, T dto)
        {
            return Mapper.Map(dto).Over(entity);
        }
    }
}