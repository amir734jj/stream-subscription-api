using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enums;
using Models.Interfaces;
using Newtonsoft.Json;

namespace Models.Models
{
    /// <summary>
    /// Streaming Subscription
    /// </summary>
    public class Stream : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        public string Url { get; set; }

        /// <summary>
        /// User reference
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        
        [Column(TypeName = "jsonb")]
        public Dictionary<int, SinkTypeEnum> SubscribedSinks  { get; set; }
    }
}