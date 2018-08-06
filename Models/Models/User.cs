using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Interfaces;

namespace Models.Models
{
    /// <summary>
    /// Website user model
    /// </summary>
    public class User : IBasicModel
    {
        [Key]
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Active { get; set; }
        
        public string Fullname { get; set; }
        
        public string Email { get; set; }
        
        public List<StreamingSubscription> Streaming { get; set; }
    }
}