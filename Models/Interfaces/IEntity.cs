using System.ComponentModel.DataAnnotations;

namespace Models.Interfaces
{
    public interface IEntity
    {
        [Key]
        int Id { get; set; }
    }
}