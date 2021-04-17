using System.Linq;
using EfCoreRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models.Sinks;

namespace Dal.Profiles
{
    public class FtpSinkProfile : IEntityProfile<FtpSink>
    {
        private readonly IEntityProfileAuxiliary _entityProfileAuxiliary;

        public FtpSinkProfile(IEntityProfileAuxiliary entityProfileAuxiliary)
        {
            _entityProfileAuxiliary = entityProfileAuxiliary;
        }
        
        public void Update(FtpSink entity, FtpSink dto)
        {
            entity.Name = dto.Name;
            entity.Host = dto.Host;
            entity.Username = dto.Username;
            entity.Password = dto.Password;
            entity.Path = dto.Path;
            entity.Port = dto.Port;
            entity.Favorite = dto.Favorite;
            entity.StreamFtpSinkRelationships =
                _entityProfileAuxiliary.ModifyList(entity.StreamFtpSinkRelationships, dto.StreamFtpSinkRelationships,
                    x => new {x.StreamId, x.FtpSinkId}).ToList();
        }

        public IQueryable<FtpSink> Include<TQueryable>(TQueryable queryable) where TQueryable : IQueryable<FtpSink>
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