using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Models.Enums;
using Models.Interfaces;
using  Models.Extensions;

namespace Models.Models
{
    /// <summary>
    /// Website user model
    /// </summary>
    public class User : IdentityUser<int>, IEntity, IEntityUpdatable<User>
    {
        public string Fullname { get; set; }

        public List<StreamingSubscription> Streaming { get; set; }
        
        public UserRoleEnum Role { get; set; }

        public User Update(User dto)
        {
            Fullname = Fullname;
            Streaming = Streaming.IdAwareUpdate(dto.Streaming, x => x.Id);
            Role = dto.Role;
            
            return this;
        }
    }
}