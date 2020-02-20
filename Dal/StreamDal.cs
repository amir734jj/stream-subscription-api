using System.Linq;
using Dal.Abstracts;
using DAL.Extensions;
using Dal.Interfaces;
using Dal.Utilities;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal
{
    public class StreamDal : BasicDalRelationalAbstract<Stream>, IStreamDal
    {
        private readonly EntityDbContext _dbContext;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="dbContext"></param>
        public StreamDal(EntityDbContext dbContext)
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
        protected override DbSet<Stream> GetDbSet()
        {
            return _dbContext.Streams;
        }

        protected override IQueryable<Stream> Intercept<TQueryable>(TQueryable queryable)
        {
            return queryable
                .Include(x => x.User)
                .ThenInclude(x => x.Streams)
                .ThenInclude(x => x.FtpSinksRelationships)
                .ThenInclude(x => x.FtpSink);
        }

        protected override Stream UpdateEntity(Stream entity, Stream dto)
        {
            entity.Url = dto.Url;
            entity.FtpSinksRelationships = entity.FtpSinksRelationships.IdAwareUpdate(dto.FtpSinksRelationships, x => x.Id);

            return entity;
        }
    }
}