﻿namespace Footballers.Data
    {
    using Footballers.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class FootballersContext : DbContext
        {
        public FootballersContext() { }

        public FootballersContext(DbContextOptions options)
            : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
            if (!optionsBuilder.IsConfigured)
                {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
                }
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<TeamFootballer>(x =>
            {
                x.HasKey(k => new { k.FootballerId, k.TeamId });
            });
            }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Footballer> Footballers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamFootballer> TeamsFootballers { get; set; }

    }
    }
