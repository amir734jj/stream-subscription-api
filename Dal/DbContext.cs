using System;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Dal
{
    public sealed class EntityDbContext: DbContext, IDataProtectionKeyContext
    {
        /// <summary>
        /// Streaming subscription table
        /// </summary>
        public DbSet<StreamingSubscription> StreamingSubscriptions { get; set; }
        
        // This maps to the table that stores keys.
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        
        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<User> Users { get; set; }

        private readonly Action<DbContextOptionsBuilder> _onConfiguring;

        /// <summary>
        /// Constructor that will be called by startup.cs
        /// </summary>
        /// <param name="dbContextOptionsBuilderAction"></param>
        public EntityDbContext(Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction)
        {
            _onConfiguring = dbContextOptionsBuilderAction;
            
            Database.EnsureCreated();
            
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => _onConfiguring(optionsBuilder);
    }
}