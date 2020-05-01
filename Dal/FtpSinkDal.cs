using System.Linq;
using Dal.Abstracts;
using Dal.Extensions;
using Dal.Interfaces;
using Dal.Utilities;
using Microsoft.EntityFrameworkCore;
using Models.Models.Sinks;

namespace Dal
{
    public class FtpSinkDal : BasicDalRelationalAbstract<FtpSink>, IFtpSinkDal
    {
        private readonly EntityDbContext _dbContext;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="dbContext"></param>
        public FtpSinkDal(EntityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the database context
        /// </summary>
        /// <returns></returns>
        protected override DbContext GetDbContext()
        {
            return _dbContext;
        }

        /// <summary>
        /// Returns DbSet
        /// </summary>
        /// <returns></returns>
        protected override DbSet<FtpSink> GetDbSet()
        {
            return _dbContext.FtpSinks;
        }

        protected override IQueryable<FtpSink> Intercept<TQueryable>(TQueryable queryable)
        {
            return queryable
                .Include(x => x.User)
                .ThenInclude(x => x.Streams)
                .ThenInclude(x => x.FtpSinkRelationships)
                .ThenInclude(x => x.FtpSink);
        }

        protected override FtpSink UpdateEntity(FtpSink entity, FtpSink dto)
        {
            entity.Name = dto.Name;
            entity.Host = dto.Host;
            entity.Username = dto.Username;
            entity.Password = dto.Password;
            entity.Path = dto.Path;
            entity.Port = dto.Port;
            entity.Favorite = dto.Favorite;
            entity.FtpSinkRelationships = entity.FtpSinkRelationships.IdAwareUpdate(dto.FtpSinkRelationships, x => x.GetHashCode());

            return entity;
        }
    }
}