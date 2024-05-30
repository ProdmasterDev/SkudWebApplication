﻿using ControllerDomain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SkudWebApplication.Db
{
    public class WebAppContext : DbContext
    {
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<ControllerLocation> ControllerLocations { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<GroupAccess> GroupAccesses { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageCard> MessageCards { get; set; }
        public DbSet<MessageOperation> MessageOperations { get; set; }
        public DbSet<QuickAccess> QuickAccess { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerGroup> WorkerGroups { get; set; }
        public DbSet<AccessMethod> AccessMethods { get; set; }
        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Worker>()
                .HasMany(w => w.Cards)
                .WithOne(c => c.Worker)
                .HasForeignKey(c => c.WorkerId)
                .HasPrincipalKey(w => w.Id);
            modelBuilder
                .Entity<Worker>()
                .HasMany(w => w.Accesses)
                .WithOne(a => a.Worker)
                .HasForeignKey(a => a.WorkerId)
                .HasPrincipalKey(w => w.Id);
            modelBuilder
                .Entity<Worker>()
                .HasMany(w => w.Events)
                .WithOne(a => a.Worker)
                .HasForeignKey(a => a.WorkerId)
                .HasPrincipalKey(w => w.Id);
            modelBuilder
                .Entity<WorkerGroup>()
                .HasMany(w => w.Workers)
                .WithOne(a => a.Group)
                .HasForeignKey(a => a.GroupId)
                .HasPrincipalKey(w => w.Id);
            modelBuilder
                .Entity<AccessMethod>()
                .HasMany(w => w.Workers)
                .WithOne(a => a.AccessMethod)
                .HasForeignKey(a => a.AccessMethodId)
                .HasPrincipalKey(w => w.Id);
            modelBuilder
                .Entity<ControllerLocation>()
                .HasMany(x => x.Accesses)
                .WithOne(x => x.ControllerLocation)
                .HasForeignKey(x => x.ControllerLocationId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder
                .Entity<ControllerLocation>()
                .HasMany(x => x.GroupAccesses)
                .WithOne(x => x.ControllerLocation)
                .HasForeignKey(x => x.ControllerLocationId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder
                .Entity<ControllerLocation>()
                .HasMany(x => x.AccessGroupAccesses)
                .WithOne(x => x.ControllerLocation)
                .HasForeignKey(x => x.ControllerLocationId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder
                .Entity<ControllerLocation>()
                .HasMany(c => c.Events)
                .WithOne(e => e.ControllerLocation)
                .HasForeignKey(e => e.ControllerLocationId)
                .HasPrincipalKey(e => e.Id);
            modelBuilder
                .Entity<EventType>()
                .HasMany(e => e.Events)
                .WithOne(e => e.EventType)
                .HasForeignKey(e => e.EventTypeId)
                .HasPrincipalKey(e => e.Id);
            modelBuilder
                .Entity<ControllerLocation>()
                .HasOne(x => x.Controller)
                .WithOne(x => x.ControllerLocation)
                .HasForeignKey<Controller>(x => x.ControllerLocationId)
                .IsRequired(false);
            modelBuilder
                .Entity<AccessGroup>()
                .HasMany(x => x.Accesses)
                .WithOne(x => x.AccessGroup)
                .HasForeignKey(x => x.AccessGroupId)
                .HasPrincipalKey(x => x.Id);
            modelBuilder
                .Entity<WorkerGroup>()
                .HasMany(x => x.GroupAccess)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}
