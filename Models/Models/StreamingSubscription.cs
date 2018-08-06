using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enums;
using Models.Interfaces;

namespace Models.Models
{
    /// <summary>
    /// Streamming Subscription
    /// </summary>
    public class StreamingSubscription : IBasicModel
    {
        [Key]
        public int Id { get; set; }
        
        public string Url { get; set; }
        
        public string Token { get; set; }
        
        [ForeignKey("User")]
        public int? UserRefId { get; set; }
        
        /// <summary>
        /// User reference
        /// </summary>
        public User User { get; set; }
        
        /// <summary>
        /// Service type
        /// </summary>
        public ServiceTypeEnum ServiceType { get; set; }
    }
}