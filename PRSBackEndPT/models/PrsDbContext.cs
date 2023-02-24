using Microsoft.EntityFrameworkCore;
using PRSBackEndPT.models;

namespace PRSBackEndPT.models
{
    public class PrsDbContext : DbContext    // not poco
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestLine> RequestLine { get; set; }

        public PrsDbContext(DbContextOptions<PrsDbContext> options) : base(options)
        {
        }
    }
}
