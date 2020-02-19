using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Interfaces;
using Models.Models;

namespace Logic.Interfaces
{
    public interface IBasicLogicUserBound<T> : IBasicLogic<T> where T: IEntity
    {
        Task<IEnumerable<T>> Get(User user, Func<T, bool> filter);
        
        Task<IEnumerable<T>> GetAll(User user);

        Task<T> Get(User user, int id);

        Task<T> Save(User user, T instance);
        
        Task<T> Delete(User user, int id);

        Task<T> Update(User user, int id, T dto);

        Task<T> Update(User user, int id, Action<T> updater);
    }
}