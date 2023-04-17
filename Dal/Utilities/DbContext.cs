using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Models.Relationships;
using Models.Models.Sinks;

namespace Dal.Utilities;

public sealed class EntityDbContext: IdentityDbContext<User, IdentityRole<int>, int>
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
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // user-stream relationship
        modelBuilder.Entity<User>()
            .HasMany(x => x.Streams)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);
            
        // user-ftp relationship
        modelBuilder.Entity<User>()
            .HasMany(x => x.FtpSinks)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);

        // stream-ftp relationship
        modelBuilder.Entity<StreamFtpSinkRelationship>()
            .HasKey(x => new {x.StreamId, x.FtpSinkId});

        modelBuilder.Entity<StreamFtpSinkRelationship>()
            .HasOne(x => x.Stream)
            .WithMany(x => x.StreamFtpSinkRelationships)
            .HasForeignKey(x => x.StreamId);
            
        modelBuilder.Entity<StreamFtpSinkRelationship>()
            .HasOne(x => x.FtpSink)
            .WithMany(x => x.StreamFtpSinkRelationships)
            .HasForeignKey(x => x.FtpSinkId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
            
        base.OnConfiguring(optionsBuilder);
    }
}