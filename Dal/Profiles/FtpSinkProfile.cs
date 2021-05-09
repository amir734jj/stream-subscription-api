using System.Linq;
using EfCoreRepository;
using EfCoreRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models.Sinks;

namespace Dal.Profiles
{
    public class FtpSinkProfile : EntityProfile<FtpSink>
    {
        public override void Update(FtpSink entity, FtpSink dto)
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

        public override IQueryable<FtpSink> Include<TQueryable>(TQueryable queryable)
        {
            return queryable
                .Include(x => x.StreamFtpSinkRelationships)
                .ThenInclude(x => x.FtpSink)
                .ThenInclude(x => x.User)
                .Include(x => x.User)
                .ThenInclude(x => x.Streams)
                .ThenInclude(x => x.StreamFtpSinkRelationships)
                .ThenInclude(x => x.FtpSink);
        }
    }
}