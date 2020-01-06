using System.ComponentModel.DataAnnotations;
using Models.Enums;
using Models.Interfaces;
using Newtonsoft.Json;

namespace Models.Models
{
    /// <summary>
    /// Streaming Subscription
    /// </summary>
    public class StreamingSubscription : IEntity, IEntityUpdatable<StreamingSubscription>
    {
        [Key]
        public int Id { get; set; }
        
        public string Url { get; set; }

        public int UserRefId { get; set; }

        /// <summary>
        /// User reference
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        
        /// <summary>
        /// Service type
        /// </summary>
        public ServiceTypeEnum ServiceType { get; set; }

        public StreamingSubscription Update(StreamingSubscription dto)
        {
            Url = dto.Url;
            ServiceType = dto.ServiceType;

            return this;
        }
    }
}