using System.ComponentModel.DataAnnotations;
using Models.Interfaces;

namespace Models.Models;

public class GlobalConfig : IEntity
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Value { get; set; } = string.Empty;
}
