using Microsoft.EntityFrameworkCore;
using PRSBackEndPT.models;

namespace PRSBackEndPT.models
{
    public class PrsDbContext : DbContext    // not poco
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public PrsDbContext(DbContextOptions<PrsDbContext> options) : base(options)
        {
        }

        public DbSet<PRSBackEndPT.models.Request> Request { get; set; }

        public DbSet<PRSBackEndPT.models.RequestLine> RequestLine { get; set; }
    }
}
