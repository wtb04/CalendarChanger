using Ical.Net;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Infrastructure
{
    public class TimeEditClient
    {
        private readonly HttpClient _httpClient;
        private readonly CalendarContext _db;

        public TimeEditClient(HttpClient httpClient, CalendarContext db)
        {
            _httpClient = httpClient;
            _db = db;
        }

        public async Task<Calendar> FetchCalendarAsync()
        {
            var urls = await _db.RoosterUrls.Where(r => !r.Hidden).Select(r => r.Url).ToListAsync();
            if (urls.Count == 0)
                throw new InvalidOperationException("No Rooster URLs defined.");

            var calendars = new List<Calendar>();
            foreach (var url in urls)
            {
                var res = await _httpClient.GetAsync(url);
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                calendars.Add(Calendar.Load(content));
            }

            var merged = new Calendar();
            foreach (var c in calendars)
                merged.Events.AddRange(c.Events);
            return merged;
        }

        public async Task<string?> GetBaseUrlAsync()
        {
            return (await _db.CalendarSettings.FirstOrDefaultAsync())?.BaseUrl;
        }
    }
}