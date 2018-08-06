using System.Collections.Generic;
using Dal.Interfaces;

namespace Logic.Abstracts
{
    public abstract class BasicLogicAbstract<T>
    {
        /// <summary>
        /// Returns instance of basic DAL
        /// </summary>
        /// <returns></returns>
        public abstract IBasicDal<T> GetBasicCrudDal();

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll() => GetBasicCrudDal().GetAll();

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get(int id) => GetBasicCrudDal().Get(id);

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual T Save(T instance) => GetBasicCrudDal().Save(instance);

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Delete(int id) => GetBasicCrudDal().Delete(id);

        /// <summary>
        /// Call forwarding
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedInstance"></param>
        /// <returns></returns>
        public virtual T Update(int id, T updatedInstance) => GetBasicCrudDal().Update(id, updatedInstance);
    }
}