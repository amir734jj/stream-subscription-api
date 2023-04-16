using System.Linq;
using EfCoreRepository;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal.Profiles;

public class UserProfile : EntityProfile<User>
{
    protected override void Update(User entity, User dto)
    {
        entity.LastLoginTime = dto.LastLoginTime;
    }

    protected override IQueryable<User> Include<TQueryable>(TQueryable queryable)
    {
        return queryable
            .Include(x => x.FtpSinks)
            .Include(x => x.Streams)
            .ThenInclude(x => x.StreamFtpSinkRelationships);
    }
}