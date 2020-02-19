using System.Linq;
using Dal.Abstracts;
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
            return queryable.Include(x => x.User);
        }
    }
}