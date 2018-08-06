using System;
using AutoMapper;
using Dal.Interfaces;
using DAL.Abstracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Models;

namespace Dal
{
    public class StreamingSubscriptionDal : BasicDalAbstract<StreamingSubscription>, IStreamingSubscriptionDal
    {
        private readonly IMapper _mapper;
        
        private readonly EntityDbContext _dbContext;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public StreamingSubscriptionDal(EntityDbContext dbContext, IMapper mapper)
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
        public override DbSet<StreamingSubscription> GetDbSet() => _dbContext.StreamingSubscriptions;
    }
}