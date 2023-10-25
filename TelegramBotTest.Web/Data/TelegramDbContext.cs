using Microsoft.EntityFrameworkCore;
using TelegramBotTest.Web.Models;

namespace TelegramBotTest.Web.Data
{
    public class TelegramDbContext : DbContext
    {
        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
