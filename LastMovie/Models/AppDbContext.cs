using Microsoft.EntityFrameworkCore;

namespace LastMovie.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<MovieProduct> MovieProductTable { get; set; }
        public DbSet<User> UserTable { get; set; }
        public DbSet<Cart> CartTable { get; set; }
        public DbSet<Tr> TrTable { get; set; }
        public DbSet<Master> MasterTable { get; set; }
        public DbSet<Comment> CommentTable { get; set; }
        public DbSet<Video> VideoTable { get; set; }
        public DbSet<Message> MessageTable { get; set; }
        public DbSet<InstallmentRecord> InstallmentRecordTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-75SCS0RS\SQLEXPRESS;Database=MyLastMovie;Trusted_Connection=True;TrustServerCertificate=true");
        }
    }
}
