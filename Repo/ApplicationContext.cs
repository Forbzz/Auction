using Microsoft.EntityFrameworkCore;
using System;
using Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Repo
{

    public class ApplicationContext: IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarLot> CarLots { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }


    }
}
