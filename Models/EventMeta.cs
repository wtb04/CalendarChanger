namespace CalendarChanger.Models
{
    public class EventMeta
    {
        public string Id { get; set; } = string.Empty;   // X-HASH-ID

        // Modification state
        public bool Hidden { get; set; }
        public TimeSpan? CustomStart { get; set; }
        public TimeSpan? CustomEnd { get; set; }
        public string? CustomTitle { get; set; }

        // Original event info from feed (persisted)
        public string? OriginalTitle { get; set; }
        public TimeSpan? OriginalStart { get; set; }
        public TimeSpan? OriginalEnd { get; set; }

        public DateOnly? Date { get; set; }
        
        public string LastHash { get; set; } = string.Empty;
    }
}