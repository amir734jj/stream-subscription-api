using System.Linq;
using EfCoreRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal.Profiles
{
    public class StreamProfile : IEntityProfile<Stream, int>
    {
        private readonly IEntityProfileAuxiliary _entityProfileAuxiliary;

        public StreamProfile(IEntityProfileAuxiliary entityProfileAuxiliary)
        {
            _entityProfileAuxiliary = entityProfileAuxiliary;
        }
        
        public Stream Update(Stream entity, Stream dto)
        {
            entity.Filter = dto.Filter;
            entity.Name = dto.Name;
            entity.Url = dto.Url;
            entity.StreamFtpSinkRelationships = _entityProfileAuxiliary.ModifyList(
                entity.StreamFtpSinkRelationships, dto.StreamFtpSinkRelationships,
                x => new {x.StreamId, x.FtpSinkId});

            return entity;
        }

        public IQueryable<Stream> Include<TQueryable>(TQueryable queryable) where TQueryable : IQueryable<Stream>
        {
            return queryable
                .Include(x => x.User)
                .ThenInclude(x => x.Streams)
                .ThenInclude(x => x.StreamFtpSinkRelationships)
                .ThenInclude(x => x.FtpSink);
        }
    }
}