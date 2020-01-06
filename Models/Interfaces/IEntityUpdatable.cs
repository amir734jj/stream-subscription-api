namespace Models.Interfaces
{
    public interface IEntityUpdatable<T>
    {
        T Update(T dto);
    }
}