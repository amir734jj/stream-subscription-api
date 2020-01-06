using Dal.Interfaces;
using DAL.Abstracts;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal
{
    public class StreamingSubscriptionDal : BasicDalAbstract<StreamingSubscription>, IStreamingSubscriptionDal
    {
        private readonly EntityDbContext _dbContext;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="dbContext"></param>
        public StreamingSubscriptionDal(EntityDbContext dbContext)
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
        protected override DbSet<StreamingSubscription> GetDbSet()
        {
            return _dbContext.StreamingSubscriptions;
        }
    }
}