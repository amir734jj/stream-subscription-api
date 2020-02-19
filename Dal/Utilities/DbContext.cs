﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Models.Models;
using Models.Models.Sinks;
using static Dal.Utilities.ConnectionStringUtility;

namespace Dal.Utilities
{
    public sealed class EntityDbContext: IdentityDbContext<User, IdentityRole<int>, int>, IDesignTimeDbContextFactory<EntityDbContext>
    {
        public DbSet<Stream> Streams { get; set; }

        public DbSet<FtpSink> FtpSinks { get; set; }
        
        public EntityDbContext() { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor that will be called by startup.cs
        /// </summary>
        /// <param name="optionsBuilderOptions"></param>
        // ReSharper disable once SuggestBaseTypeForParameter
        public EntityDbContext(DbContextOptions<EntityDbContext> optionsBuilderOptions) : base(optionsBuilderOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Streams)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                .HasMany(x => x.FtpSinks)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
        }

        
        /// <summary>
        ///     This is used for DB migration locally
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public EntityDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var options = new DbContextOptionsBuilder<EntityDbContext>()
                .UseNpgsql(ConnectionStringUrlToResource(configuration.GetValue<string>("DATABASE_URL")))
                .Options;

            return new EntityDbContext(options);
        }
    }
}