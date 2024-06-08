using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Diagnostics.Metrics;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using dotnet_facebook.Models.DatabaseObjects.Roles;

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

            modelBuilder.Entity<Like>()
                .HasOne(Like => Like.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrivateMessage>()
                .HasOne(PrivateMessage => PrivateMessage.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrivateMessage>()
                .HasOne(PrivateMessage => PrivateMessage.Receiver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MainPost>()
                .HasMany(MainPost => MainPost.Tags)
                .WithMany()
                .UsingEntity(j => j.ToTable("MainPostTag"));

            modelBuilder.Entity<MainPost>()
                .HasMany(MainPost => MainPost.Comments)
                .WithOne()
                .HasForeignKey(Comment => Comment.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(Comment => Comment.ParentPost)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasMany(MainPost => MainPost.Tags)
                .WithMany()
                .UsingEntity(j => j.ToTable("GroupTag"));

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User1)
                .WithMany()
                .HasForeignKey(f => f.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User2)
                .WithMany()
                .HasForeignKey(f => f.User2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<MainPost> MainPosts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<SiteRole> SiteRoles { get; set; }
        public virtual DbSet<UserSiteRole> UserSiteRoles { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<PrivateMessage> PrivateMessages { get; set; }

        public virtual DbSet<GroupUser> GroupUsers { get; set; }
    }
}
