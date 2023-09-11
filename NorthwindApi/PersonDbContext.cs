using Microsoft.EntityFrameworkCore;

namespace NorthwindApi
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Person { get; set; }
    }
}
