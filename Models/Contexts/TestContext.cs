using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Diagnostics.Metrics;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using dotnet_facebook.Models.DatabaseObjects.Roles;
using dotnet_facebook.Migrations;
using dotnet_facebook.Utils;
using System.Data;

namespace dotnet_facebook.Models.Contexts
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options) : base(options)
        {
            // check if Models/Contexts/dbcreated.txt exists
            // if it doesn't, create the default roles (user, admin) and admin user and create the file

            if (!File.Exists("Models/Contexts/dbcreated.txt"))
            {
                Console.WriteLine("Creating default roles and admin user");

                var userRole = new SiteRole
                {
                    SiteRoleName = "User",
                    IsDefault = true,
                    AdministrativePerms = false,
                    SiteRoleId = 0
                };

                var adminRole = new SiteRole
                {
                    SiteRoleName = "Admin",
                    IsDefault = true,
                    AdministrativePerms = true,
                    SiteRoleId = 1
                };

                SiteRoles.Add(userRole);
                SiteRoles.Add(adminRole);

                var adminUser = new User
                {
                    Nickname = "admin123",
                    HashedPassword = PasswordHash.Create("admin123"),
                    Password = "",
                    AccountCreationDate = DateTime.Now,
                    UserId = 0,
                };

                adminUser.UserProfile = new UserProfile
                {
                    User = adminUser,
                    UserBio = "Hey, I'm a user!"
                };

                Users.Add(adminUser);

                UserSiteRoles.Add(new UserSiteRole()
                {
                    User = adminUser,
                    Role = userRole
                });

                UserSiteRoles.Add(new UserSiteRole()
                {
                    User = adminUser,
                    Role = adminRole
                });

                SaveChanges();

                File.Create("Models/Contexts/dbcreated.txt");
            }
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
