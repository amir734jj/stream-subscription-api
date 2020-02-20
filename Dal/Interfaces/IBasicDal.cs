using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IBasicDal<T>
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter);
        
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task<T> Save(T instance);
        
        Task<T> Delete(int id);

        Task<T> Update(int id, T dto);

        Task<T> Update(int id, Action<T> updater);
    }
}