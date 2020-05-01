using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Interfaces;
using Newtonsoft.Json;

namespace Models.Models.Sinks
{
    public class FtpSink : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Host { get; set; }

        public int Port { get; set; } = 21;

        public string Path { get; set; } = string.Empty;

        public bool Favorite { get; set; }
        
        /// <summary>
        /// User reference
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        
        public List<StreamFtpSinkRelationship> FtpSinkRelationships  { get; set; } = new List<StreamFtpSinkRelationship>();
    }
}