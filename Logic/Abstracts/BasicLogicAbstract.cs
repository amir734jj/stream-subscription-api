using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Interfaces;
using EfCoreRepository.Interfaces;
using Logic.Interfaces;
using Models.Interfaces;

namespace Logic.Abstracts
{
    public abstract class BasicLogicAbstract<T> : IBasicLogic<T> where T: class, IEntity
    {
        /// <summary>
        /// Returns instance of basic DAL
        /// </summary>
        /// <returns></returns>
        protected abstract IBasicCrud<T, int> GetBasicCrudDal();

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> GetAll()
        {
            return GetBasicCrudDal().GetAll();
        }

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> Get(int id)
        {
            return GetBasicCrudDal().Get(id);
        }

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual Task<T> Save(T instance)
        {
            return GetBasicCrudDal().Save(instance);
        }

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> Delete(int id)
        {
            return GetBasicCrudDal().Delete(id);
        }

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual Task<T> Update(int id, T dto)
        {
            return GetBasicCrudDal().Update(id, dto);
        }

        public async Task<T> Update(int id, Action<T> updater)
        {
            var entity = await Get(id);

            updater(entity);

            return await Update(id, entity);
        }
    }
}