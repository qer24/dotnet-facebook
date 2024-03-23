﻿using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Diagnostics.Metrics;

namespace dotnet_facebook.Models.Contexts
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options) : base(options)
        {

        }
        protected TestContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.UserProfile)
                .WithOne(a => a.User);

            modelBuilder.Entity<Comment>()
                .HasOne(Comment => Comment.ParentPost)
                .WithMany()
                .HasForeignKey(Comment => Comment.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(Like => Like.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasOne(Group => Group.OwnerUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MainPost> MainPosts { get; set; }
    }
}
