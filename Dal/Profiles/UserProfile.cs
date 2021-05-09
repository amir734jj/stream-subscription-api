using System.Linq;
using EfCoreRepository;
using EfCoreRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal.Profiles
{
    public class UserProfile : EntityProfile<User>
    {
        public override void Update(User entity, User dto)
        {
            entity.LastLoginTime = dto.LastLoginTime;
        }

        public override IQueryable<User> Include<TQueryable>(TQueryable queryable)
        {
            return queryable
                .Include(x => x.FtpSinks)
                .Include(x => x.Streams)
                .ThenInclude(x => x.StreamFtpSinkRelationships);
        }
    }
}