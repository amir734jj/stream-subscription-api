using System.Linq;
using EfCoreRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal.Profiles
{
    public class UserProfile : IEntityProfile<User, int>
    {
        public User Update(User entity, User dto)
        {
            entity.LastLoginTime = dto.LastLoginTime;

            return entity;
        }

        public IQueryable<User> Include<TQueryable>(TQueryable queryable) where TQueryable : IQueryable<User>
        {
            return queryable
                .Include(x => x.FtpSinks)
                .Include(x => x.Streams)
                .ThenInclude(x => x.StreamFtpSinkRelationships);
        }
    }
}