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

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MainPost> MainPosts { get; set; }
    }
}
