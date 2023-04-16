using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Interfaces;
using Models.Models.Relationships;
using Newtonsoft.Json;

namespace Models.Models;

/// <summary>
/// Streaming Subscription
/// </summary>
public class Stream : IEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
        
    public string Url { get; set; }

    public string Filter { get; set; } = "advertisement";

    /// <summary>
    /// User reference
    /// </summary>
    [JsonIgnore]
    public User User { get; set; }
        
    public List<StreamFtpSinkRelationship> StreamFtpSinkRelationships  { get; set; } = new List<StreamFtpSinkRelationship>();
}