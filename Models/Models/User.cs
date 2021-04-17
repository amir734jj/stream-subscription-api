using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Models.Interfaces;
using Models.Models.Sinks;

namespace Models.Models
{
    /// <summary>
    /// Website user model
    /// </summary>
    public class User : IdentityUser<int>, IEntity
    {
        [Key]
        [PersonalData]
        public override int Id { get; set; }

        public string Name { get; set; }
        
        public virtual DateTimeOffset LastLoginTime { get; set; }  = DateTimeOffset.MinValue;

        public List<Stream> Streams { get; set; } = new List<Stream>();

        /// <summary>
        /// FtpSink - User relationship
        /// </summary>
        public List<FtpSink> FtpSinks { get; set; } = new List<FtpSink>();

        public object Obfuscate()
        {
            const string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";

            var obfuscatedEmail = Regex.Replace(Email, pattern, m => new string('*', m.Length));
            
            return new {Email = obfuscatedEmail, Name};
        }

        public object ToAnonymousObject()
        {
            return new {Email, Name};
        }
    }
}