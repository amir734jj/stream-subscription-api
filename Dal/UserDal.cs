using Dal.Abstracts;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Dal
{
    public class UserDal : BasicDalAbstract<User>, IUserDal
    {
        private readonly EntityDbContext _dbContext;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="dbContext"></param>
        public UserDal(EntityDbContext dbContext)
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
        protected override DbSet<User> GetDbSet()
        {
            return _dbContext.Users;
        }
    }
}