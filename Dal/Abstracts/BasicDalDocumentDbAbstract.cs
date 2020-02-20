using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dal.Interfaces;
using Marten;
using Models.Interfaces;

namespace Dal.Abstracts
{
    public abstract class BasicDalDocumentDbAbstract<T> : IBasicDal<T> where T : class, IEntity
    {
        /// <summary>
        ///     Document store
        /// </summary>
        /// <returns></returns>
        protected abstract DocumentStore ResolveStore();
        
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter)
        {
            using var session = ResolveStore().LightweightSession();

            var result = await session.Query<T>().Where(filter).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using var session = ResolveStore().LightweightSession();

            var result = await session.Query<T>().ToListAsync();

            return result;
        }

        public async Task<T> Get(int id)
        {
            using var session = ResolveStore().LightweightSession();

            var result = await session.Query<T>().Where(x => x.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<T> Save(T instance)
        {
            using var session = ResolveStore().LightweightSession();

            session.Insert(instance);

            await session.SaveChangesAsync();

            return instance;
        }

        public async Task<T> Delete(int id)
        {
            var instance = await Get(id);
            
            using var session = ResolveStore().LightweightSession();

            session.DeleteWhere<T>(x => x.Id == id);

            await session.SaveChangesAsync();

            return instance;
        }

        public async Task<T> Update(int id, T dto)
        {
            var instance = await Get(id);
            
            using var session = ResolveStore().LightweightSession();

            session.Update(dto);

            await session.SaveChangesAsync();

            return instance;
        }

        public async Task<T> Update(int id, Action<T> updater)
        {
            var instance = await Get(id);

            updater(instance);
            
            return await Update(id, instance);
        }
    }
}