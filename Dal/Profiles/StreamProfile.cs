using System.Linq;
using EfCoreRepository;
using EfCoreRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal.Profiles
{
    public class StreamProfile : EntityProfile<Stream>
    {
        public override void Update(Stream entity, Stream dto)
        {
            entity.Filter = dto.Filter;
            entity.Name = dto.Name;
            entity.Url = dto.Url;
            ModifyList(
                entity.StreamFtpSinkRelationships,
                dto.StreamFtpSinkRelationships,
                x => (x.StreamId, x.FtpSinkId));
        }

        public override IQueryable<Stream> Include<TQueryable>(TQueryable queryable)
        {
            return queryable
                .Include(x => x.User)
                .ThenInclude(x => x.Streams)
                .ThenInclude(x => x.StreamFtpSinkRelationships)
                .ThenInclude(x => x.FtpSink);
        }
    }
}