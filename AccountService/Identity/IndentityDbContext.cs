using AccountService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Identity
{
    public class IndentityDbContext : IdentityDbContext<AppUser>
    {
        public IndentityDbContext(DbContextOptions<IndentityDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
