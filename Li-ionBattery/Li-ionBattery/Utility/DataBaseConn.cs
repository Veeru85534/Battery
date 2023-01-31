using Li_ionBattery.Models;
using Microsoft.EntityFrameworkCore;

namespace Li_ionBattery.Utility
{
    public class DataBaseConn : DbContext
    {
        public DataBaseConn(DbContextOptions<DataBaseConn> options) : base(options) { }
        public DbSet<Battery> Li_ionBattery { get; set; }
    }
}
