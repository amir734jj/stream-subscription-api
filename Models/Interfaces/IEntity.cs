using System.ComponentModel.DataAnnotations;
using EfCoreRepository.Interfaces;

namespace Models.Interfaces
{
    public interface IEntity : IEntity<int>
    {
    }
}