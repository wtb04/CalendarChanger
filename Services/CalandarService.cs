using System.Text;
using System.Text.RegularExpressions;
using CalendarChanger.Infrastructure;
using CalendarChanger.Models;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Services
{
    public interface ICalendarService
    {
        Task<byte[]> GetCalendarAsync(string calendarTypeName, IEnumerable<string>? includeTypes = null, IEnumerable<string>? excludeTypes = null);
    }

    public class CalendarService : ICalendarService
    {
        private readonly TimeEditClient _client;
        private readonly CalendarContext _db;

        public CalendarService(TimeEditClient client, CalendarContext db)
        {
            _client = client;
            _db = db;
        }

        public async Task<byte[]> GetCalendarAsync(
            string calendarTypeName,
            IEnumerable<string>? includeTypes = null,
            IEnumerable<string>? excludeTypes = null)
        {
            var baseUrl = (await _db.CalendarSettings.FirstOrDefaultAsync())?.BaseUrl
                          ?? "http://url-not-configured";

            var calendar = await _client.FetchCalendarAsync();
            calendar.Properties.Add(new CalendarProperty("X-WR-CALNAME", $"UTwente {calendarTypeName}"));

            var processed = await Task.WhenAll(
                calendar.Events.Select(e => e.ProcessAsync(baseUrl, _db))
            );

            var events = processed
                .Where(e => !(e.IsAllDay && e.Description.Contains("Event type cannot be found")))
                .ToList();

            var includeSet = includeTypes is null
                ? null
                : new HashSet<string>(includeTypes, StringComparer.OrdinalIgnoreCase);

            var excludeSet = excludeTypes is null
                ? null
                : new HashSet<string>(excludeTypes, StringComparer.OrdinalIgnoreCase);

            if (includeSet is not null && includeSet.Count > 0)
            {
                events = events.Where(e => includeSet.Contains(e.GetOriginalEventType())).ToList();
            }

            if (excludeSet is not null && excludeSet.Count > 0)
            {
                events = events.Where(e => !excludeSet.Contains(e.GetOriginalEventType())).ToList();
            }

            
            var metaMap = _db.Events.ToDictionary(m => m.Id, m => m);

            events = events
                .Select(e =>
                {
                    var id = e.Properties.FirstOrDefault(p => p.Name == "X-HASH-ID")?.Value?.ToString();
                    if (id == null)
                        return e;

                    if (!metaMap.TryGetValue(id, out var meta))
                        return e;

                    var origTitle = e.Summary;
                    var origStart = e.Start.AsSystemLocal.TimeOfDay;
                    var origEnd = e.End.AsSystemLocal.TimeOfDay;
                    var origDate = DateOnly.FromDateTime(e.Start.AsSystemLocal.Date);
                    
                    if (meta.OriginalTitle == null) meta.OriginalTitle = origTitle;
                    if (meta.OriginalStart == null) meta.OriginalStart = origStart;
                    if (meta.OriginalEnd == null) meta.OriginalEnd = origEnd;
                    if (meta.Date == null) meta.Date = origDate;
                    
                    if (meta.Hidden)
                        return null;

                    var notes = new List<string>();

                    if (!string.IsNullOrWhiteSpace(meta.CustomTitle) && meta.CustomTitle != e.Summary)
                    {
                        notes.Add($"Title changed (original: {meta.OriginalTitle})");
                        e.Summary = meta.CustomTitle;
                    }

                    if (meta.CustomStart.HasValue)
                    {
                        var newStart = e.Start.AsSystemLocal.Date + meta.CustomStart.Value;
                        if (newStart != e.Start.AsSystemLocal)
                        {
                            notes.Add($"Start time changed (original: {meta.OriginalStart:hh\\:mm})");
                            e.Start = new CalDateTime(newStart.ToUniversalTime());
                        }
                    }

                    if (meta.CustomEnd.HasValue)
                    {
                        var newEnd = e.End.AsSystemLocal.Date + meta.CustomEnd.Value;
                        if (newEnd != e.End.AsSystemLocal)
                        {
                            notes.Add($"End time changed (original: {meta.OriginalEnd:hh\\:mm})");
                            e.End = new CalDateTime(newEnd.ToUniversalTime());
                        }
                    }

                    if (notes.Count > 0)
                    {
                        var prefix = string.IsNullOrWhiteSpace(e.Description) ? "" : "\n\n";
                        e.Description = $"{e.Description}{prefix}User Modifications:\n{string.Join("\n", notes)}";
                    }

                    return e;
                })
                .Where(e => e != null)
                .ToList()!;

            await _db.SaveChangesAsync();

            calendar.Events.Clear();
            calendar.Events.AddRange(events);

            var serializer = new CalendarSerializer();
            var serialized = serializer.SerializeToString(calendar);

            serialized = Regex.Replace(
                serialized,
                @"geo:([-+]?\d+(\.\d+)?)\\,([-+]?\d+(\.\d+)?)",
                "geo:$1,$3"
            );

            return Encoding.UTF8.GetBytes(serialized);
        }
    }
}
