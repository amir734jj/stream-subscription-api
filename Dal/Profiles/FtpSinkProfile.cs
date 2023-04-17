using System.Linq;
using EfCoreRepository;
using Microsoft.EntityFrameworkCore;
using Models.Models.Sinks;

namespace Dal.Profiles;

public class FtpSinkProfile : EntityProfile<FtpSink>
{
    protected override void Update(FtpSink entity, FtpSink dto)
    {
        entity.Name = dto.Name;
        entity.Host = dto.Host;
        entity.Username = dto.Username;
        entity.Password = dto.Password;
        entity.Path = dto.Path;
        entity.Port = dto.Port;
        entity.Favorite = dto.Favorite;
        ModifyList(
            entity.StreamFtpSinkRelationships,
            dto.StreamFtpSinkRelationships,
            x => (x.StreamId, x.FtpSinkId));
    }

    protected override IQueryable<FtpSink> Include<TQueryable>(TQueryable queryable)
    {
        return queryable
            .Include(x => x.StreamFtpSinkRelationships)
            .Include(x => x.User)
            .ThenInclude(x => x.Streams)
            .ThenInclude(x => x.StreamFtpSinkRelationships)
            .ThenInclude(x => x.FtpSink);
    }
}