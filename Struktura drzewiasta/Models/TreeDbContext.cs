using Microsoft.EntityFrameworkCore;

namespace Struktura_drzewiasta.Models
{
    public class TreeDbContext : DbContext
    { 
        public TreeDbContext(DbContextOptions<TreeDbContext> options) : base(options)
        {

        }
        public DbSet<Node> Nodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("DefaultConnection");
            }
        }
    }
}
