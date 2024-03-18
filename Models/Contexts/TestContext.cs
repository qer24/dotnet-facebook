using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<Group>()
                .HasMany(groups => groups.Users)
                .WithMany(users => users.Groups)
                .UsingEntity("UsersToGroupsJoinTable");

            modelBuilder.Entity<Group>()
                .HasMany(groups => groups.Moderators)
                .WithMany(users => users.Groups)
                .UsingEntity("ModeratorsToGroupsJoinTable");

            modelBuilder.Entity<Group>()
                .HasOne(groups => groups.OwnerUser)
                .WithMany(user => user.Groups)
                .IsRequired();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MainPost> MainPosts { get; set; }
    }
}
