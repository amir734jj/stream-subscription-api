using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dal.Interfaces;
using DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;


namespace DAL.Abstracts
{
    public abstract class BasicDalAbstract<T> : IBasicDal<T> where T : class, IBasicModel
    {
        /// <summary>
        /// Abstract to get IMapper
        /// </summary>
        /// <returns></returns>
        public abstract IMapper GetMapper();
        
        /// <summary>
        /// Abstract to get database context
        /// </summary>
        /// <returns></returns>
        public abstract DbContext GetDbContext();
        
        /// <summary>
        /// Abstract to get entity set
        /// </summary>
        /// <returns></returns>
        public abstract DbSet<T> GetDbSet();
        
        /// <summary>
        /// Returns all enities
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll() => GetDbSet().ToList();

        /// <summary>
        /// Returns an entity given the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get(int id) => GetDbSet().FirstOrDefaultCache(x => x.Id == id);

        /// <summary>
        /// Saves an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual T Save(T instance)
        {
            GetDbSet().Add(instance);
            GetDbContext().SaveChanges();
            return instance;
        }

        /// <summary>
        /// Deletes enitity given the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Delete(int id)
        {
            var instance = GetDbSet().FirstOrDefaultCache(x => x.Id == id);

            if (instance != null)
            {
                GetDbSet().Remove(instance);
                GetDbContext().SaveChanges();
                return instance;
            }

            return null;
        }

        /// <summary>
        /// Updates enity given the id and new instance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedInstance"></param>
        /// <returns></returns>
        public virtual T Update(int id, T updatedInstance)
        {
            var instance = GetDbSet().FirstOrDefaultCache(x => x.Id == id);

            if (instance != null)
            {
                // Copy the fields
                instance = GetMapper().Map(updatedInstance, instance);
                GetDbSet().Update(instance);
                GetDbContext().SaveChanges();
                return updatedInstance;
            }

            return null;
        }        
    }
}