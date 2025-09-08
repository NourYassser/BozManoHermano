using BOZMANOHERMANO.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StartUp.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FollowedId });

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Followings)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FollowedId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Posts> Posts { get; set; }

        public DbSet<Comments> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Retweets> Retweets { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }

        public DbSet<UserDM> UserDM { get; set; }
    }
}
