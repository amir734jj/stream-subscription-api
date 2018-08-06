using System.Collections.Generic;

namespace Logic.Interfaces
{
    public interface IBasicLogic<T>
    {
        IEnumerable<T> GetAll();

        T Get(int id);

        T Save(T instance);
        
        T Delete(int id);

        T Update(int id, T updatedInstance);
    }
}