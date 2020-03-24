using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Models.Interfaces;

namespace Models.Models
{
    /// <summary>
    /// Website user model
    /// </summary>
    public class User : IdentityUser<int>, IEntity
    {
        public string Fullname { get; set; }

        public List<Stream> Streams { get; set; }
    }
}