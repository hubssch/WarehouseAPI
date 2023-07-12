using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Models;

namespace WarehouseAPI.Data
{
    public class WarehouseAppDbContext : DbContext
    {
        public WarehouseAppDbContext(DbContextOptions<WarehouseAppDbContext> options) : base(options) { }

        public DbSet<DbDocument> Documents { get; set; }
        public DbSet<DbItem> Items { get; set; }
        public DbSet<DbArticle> Articles { get; set; }
        public DbSet<DbContract> Contracts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
