using System.ComponentModel.DataAnnotations;
using Models.Enums;
using Models.Interfaces;
using Newtonsoft.Json;

namespace Models.Models
{
    /// <summary>
    /// Streaming Subscription
    /// </summary>
    public class Stream : IEntity, IEntityUpdatable<Stream>
    {
        [Key]
        public int Id { get; set; }
        
        public string Url { get; set; }

        /// <summary>
        /// User reference
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        
        /// <summary>
        /// Service type
        /// </summary>
        public ServiceTypeEnum ServiceType { get; set; }

        public Stream Update(Stream dto)
        {
            Url = dto.Url;
            ServiceType = dto.ServiceType;

            return this;
        }
    }
}