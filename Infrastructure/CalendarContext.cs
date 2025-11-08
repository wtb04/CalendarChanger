using CalendarChanger.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Infrastructure
{
    public class CalendarContext : DbContext
    {
        public CalendarContext(DbContextOptions<CalendarContext> options)
            : base(options)
        {
        }

        public DbSet<EventMeta> Events { get; set; } = null!;
        public DbSet<CalendarFilter> CalendarFilters { get; set; } = null!;
        public DbSet<CalendarSettings> CalendarSettings { get; set; } = null!;
        public DbSet<RoosterUrl> RoosterUrls { get; set; } = null!;
        public DbSet<RenameRule> RenameRules { get; set; } = null!;

    }
}