using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var ReaderId = "4d852221-1eba-48e1-89ee-66d6a8b6234f";
            var WriterId = "dda552cb-9cc2-4de2-8b8f-a721c4ac8b9c";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id=ReaderId,
                    ConcurrencyStamp=ReaderId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                },
                new IdentityRole()
                {
                    Id=WriterId,
                    ConcurrencyStamp=WriterId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
