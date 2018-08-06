using System.Collections.Generic;

namespace Dal.Interfaces
{
    public interface IBasicDal<T>
    {
        IEnumerable<T> GetAll();

        T Get(int id);

        T Save(T instance);
        
        T Delete(int id);

        T Update(int id, T updatedInstance);
    }
}