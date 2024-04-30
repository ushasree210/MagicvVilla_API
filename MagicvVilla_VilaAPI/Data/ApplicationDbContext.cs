using MagicvVilla_VilaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicvVilla_VilaAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
    }
}
