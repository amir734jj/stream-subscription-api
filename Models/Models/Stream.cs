﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string Name { get; set; }
        
        public string Url { get; set; }

        /// <summary>
        /// User reference
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }

        public List<StreamFtpSinkRelationship> FtpSinkRelationships  { get; set; } = new List<StreamFtpSinkRelationship>();
    }
}