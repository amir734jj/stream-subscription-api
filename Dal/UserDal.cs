using System;
using AutoMapper;
using Dal.Interfaces;
using DAL.Abstracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Models;

namespace Dal
{
    public class UserDal : BasicDalAbstract<User>, IUserDal
    {
        private readonly IMapper _mapper;
        
        private readonly EntityDbContext _dbContext;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public UserDal(EntityDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Instance of AutoMapper
        /// </summary>
        /// <returns></returns>
        public override IMapper GetMapper() => _mapper;

        /// <summary>
        /// Returns the database context
        /// </summary>
        /// <returns></returns>
        public override DbContext GetDbContext() => _dbContext;

        /// <summary>
        /// Returns DbSet
        /// </summary>
        /// <returns></returns>
        public override DbSet<User> GetDbSet() => _dbContext.Users;
    }
}