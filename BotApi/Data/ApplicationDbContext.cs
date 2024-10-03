using BotApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BotApi.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> User { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }
}
