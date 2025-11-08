using System.Security.Cryptography;
using System.Text;
using Ical.Net.CalendarComponents;

namespace CalendarChanger.Services;

public class EventHasher
{
    public static string? Compute(CalendarEvent e)
    {
        var title = e.Summary?.Trim();
        var start = e.Start?.AsSystemLocal;
        var end = e.End?.AsSystemLocal;
        var location = e.Location?.Trim();
        var attendees = e.Attendees?.Select(a => a.CommonName?.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
        var type = e.Properties.FirstOrDefault(p => p.Name == "X-ORIGINAL-TYPE")?.Value?.ToString()?.Trim();

        if (string.IsNullOrEmpty(title) ||
            start == null ||
            end == null ||
            string.IsNullOrEmpty(location) ||
            attendees == null || attendees.Count == 0 ||
            string.IsNullOrEmpty(type))
            return null;

        var key = string.Join("|", new[]
        {
            title,
            start.Value.ToString("O"),
            end.Value.ToString("O"),
            location,
            string.Join(",", attendees),
            type
        });

        using var sha1 = SHA1.Create();
        return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(key)));
    }
}