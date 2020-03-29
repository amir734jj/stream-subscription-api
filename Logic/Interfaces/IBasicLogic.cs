using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Interfaces;

namespace Logic.Interfaces
{
    public interface IBasicLogic<T> where T: IEntity
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task<T> Save(T instance);
        
        Task<T> Delete(int id);

        Task<T> Update(int id, T dto);

        Task<T> Update(int id, Action<T> updater);
    }
}