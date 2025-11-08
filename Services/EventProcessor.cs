using System.Globalization;
using System.Text.RegularExpressions;
using CalendarChanger.Helpers;
using CalendarChanger.Infrastructure;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Services
{
    public static class EventProcessor
    {
        public static async Task<CalendarEvent> ProcessAsync(this CalendarEvent ev, string baseUrl, CalendarContext db)
        {
            return (await ev.AddAttendees()
                    .AddAppleLocation()
                    .SetTitleAsync(db))
                    .SetDescription(baseUrl)
                    .AddHashId(baseUrl);
        }
            

        private static CalendarEvent AddAttendees(this CalendarEvent ev)
        {
            if (string.IsNullOrWhiteSpace(ev.Description)) return ev;

            var lines = ev.Description.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) return ev;

            var members = lines[0]
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Trim(',', ' ', '\\'))
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToArray();

            ev.Attendees = members.Select(m => new Attendee
            {
                CommonName = m,
                ParticipationStatus = "ACCEPTED",
                Value = new Uri($"mailto:{m.Replace(" ", "").ToLowerInvariant()}@utwente.nl")
            }).ToList();

            ev.Description = string.Join("\n", lines.Skip(1));
            return ev;
        }

        private static CalendarEvent AddAppleLocation(this CalendarEvent ev)
        {
            if (string.IsNullOrEmpty(ev.Location)) return ev;

            ev.Location = ev.Location.Replace("Zaal: ", "");
            var location = SchoolLocation.GetAppleLocation(ev.Location);
            if (location == null) return ev;

            var appleProp =
                $"X-APPLE-STRUCTURED-LOCATION;VALUE=URI;" +
                $"X-APPLE-MAPKIT-HANDLE={location.AppleLocation};" +
                $"X-APPLE-RADIUS=80;X-APPLE-REFERENCEFRAME=1;" +
                $"X-TITLE=\"{ev.Location}\":geo";

            ev.AddProperty(appleProp, location.Geo);
            return ev;
        }

        private static async Task<CalendarEvent> SetTitleAsync(this CalendarEvent ev, CalendarContext db)
        {
            ev.Summary = Regex.Replace(ev.Summary, @"\s*\d{6,}.*", "");
            ev.Summary = Regex.Replace(ev.Summary, @"[.\s]+$", "");

            var rules = await db.RenameRules.ToListAsync();
            foreach (var rule in rules.Where(rule => Regex.IsMatch(ev.Summary, rule.Match, RegexOptions.IgnoreCase)))
            {
                ev.Summary = Regex.Replace(ev.Summary, rule.Match, rule.Replace, RegexOptions.IgnoreCase);
                break;
            }

            return ev;
        }

        private static CalendarEvent SetDescription(this CalendarEvent ev, string baseUrl)
        {
            var type = ev.GetEventType();
            ev.Properties.Add(new CalendarProperty("X-ORIGINAL-TYPE", type));
            
            var culture = new CultureInfo("nl-NL");
            var extra = ev.GetExtraInformation();

            ev.Description =
                $"{(string.IsNullOrEmpty(extra) ? "" : "Added information:\n" + extra + "\n\n")}" +
                $"Sync: {DateTime.Now.ToString(culture)}\n" +
                $"Type: {type}\n" +
                $"© Wouter ten Brinke - {DateTime.Now.Year}\n\n" +
                $"Map: {baseUrl}/MazeMap/{ev.Location.Split('+')[0].Replace(" ", "")}\n";


            return ev;
        }

        private static string GetExtraInformation(this CalendarEvent ev) =>
            ev.Description.Contains("Extra Info:")
                ? ev.Description.Split("Extra Info: ")[1].Split("\n")[0]
                : "";

        private static string GetEventType(this CalendarEvent ev)
        {
            var lines = ev.Description.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var idLine = lines
                .Select((line, i) => new { line, i })
                .FirstOrDefault(l => l.line.TrimStart().StartsWith("ID", StringComparison.OrdinalIgnoreCase));

            return idLine is { i: > 0 }
                ? lines[idLine.i - 1].Trim()
                : "Event type cannot be found";
        }
        
        public static string GetOriginalEventType(this CalendarEvent ev)
        {
            var prop = ev.Properties.FirstOrDefault(p => p.Name == "X-ORIGINAL-TYPE");
            return prop?.Value.ToString() ?? "Event type cannot be found";
        }

        private static CalendarEvent AddHashId(this CalendarEvent ev, string baseUrl)
        {
            var hash = EventHasher.Compute(ev);
            if (string.IsNullOrEmpty(hash)) return ev;
            
            ev.AddProperty("X-HASH-ID", hash);
            var editUrl = $"{baseUrl}/EventModifier/Edit/{hash}";

            // Append cleanly
            ev.Description += $"\nEdit: {editUrl}";
            return ev;
        }
    }
}
