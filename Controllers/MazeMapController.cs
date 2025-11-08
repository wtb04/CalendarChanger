using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CalendarChanger.Controllers;
[ApiController]
[Route("[controller]")]
public class MazeMapController : ControllerBase
{
    private static readonly JsonElement[] Rooms;
    private static readonly Dictionary<string, int> ShortToBuilding = new();
    private static readonly Dictionary<int, string> BuildingIdToShort = new();

    static MazeMapController()
    {
        // preload data
        var roomsJson = System.IO.File.ReadAllText("mazemap/rooms.json");
        var buildingJson = System.IO.File.ReadAllText("mazemap/building_shortnames.json");

        Rooms = JsonSerializer.Deserialize<JsonElement[]>(roomsJson)!;
        var buildings = JsonSerializer.Deserialize<JsonElement[]>(buildingJson)!;

        foreach (var b in buildings)
        {
            var bid = b.GetProperty("buildingId").GetInt32();
            var shortNames = b.GetProperty("shortNames").EnumerateArray().Select(x => x.GetString() ?? "").ToList();

            foreach (var s in shortNames)
                ShortToBuilding[s.ToLower()] = bid;

            if (shortNames.Count > 0)
                BuildingIdToShort[bid] = shortNames[0];
        }
    }

    [HttpGet("{q}")]
    public IActionResult Get(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Query required.");

        var query = q.Trim().ToLowerInvariant();
        var digits = Regex.Replace(query, @"\D", "");

        var prefixMatch = Regex.Match(query, @"^([a-zA-Z]+)");
        var buildingRooms = Rooms.AsEnumerable();

        if (prefixMatch.Success)
        {
            var prefix = prefixMatch.Groups[1].Value.ToLower();
            if (ShortToBuilding.TryGetValue(prefix, out var bid))
            {
                buildingRooms = buildingRooms.Where(r =>
                    r.TryGetProperty("buildingId", out var v) && v.GetInt32() == bid);
            }
        }

        // core helpers
        static string CleanTitle(JsonElement r)
        {
            var t = r.TryGetProperty("title", out var v) ? v.GetString() ?? "" : "";
            return Regex.Replace(t, @"^\s*collegezaal[:\-\s]+", "", RegexOptions.IgnoreCase).Trim();
        }

        string PrefixedTitle(JsonElement r)
        {
            var baseTitle = CleanTitle(r);
            var bid = r.TryGetProperty("buildingId", out var v) ? v.GetInt32() : 0;
            var prefix = BuildingIdToShort.GetValueOrDefault(bid, "");
            return $"{prefix}{baseTitle}".Trim();
        }

        // search order
        var titleMatches = buildingRooms.Where(r =>
            CleanTitle(r).ToLower().Contains(query)).ToList();

        if (!titleMatches.Any())
            titleMatches = buildingRooms.Where(r =>
                PrefixedTitle(r).ToLower().Contains(query)).ToList();

        if (!titleMatches.Any())
            titleMatches = buildingRooms.Where(r =>
                r.TryGetProperty("identifier", out var id) &&
                (id.GetString() ?? "").ToLower().Contains(query)).ToList();

        if (!titleMatches.Any() && digits.Length > 0)
            titleMatches = buildingRooms.Where(r =>
                Regex.Replace(r.GetProperty("title").GetString() ?? "", @"\D", "") == digits).ToList();

        if (!titleMatches.Any())
            return NotFound("No matches.");

        var best = titleMatches.OrderBy(r => (r.GetProperty("title").GetString() ?? "").Length).First();
        var identifier = best.TryGetProperty("identifier", out var idVal) ? idVal.GetString() : null;

        if (string.IsNullOrEmpty(identifier))
            return NotFound("No identifier for match.");

        var redirectUrl = $"https://use.mazemap.com/?utm_medium=longurl#v=1&campusid=171&sharepoitype=identifier&sharepoi={identifier}";
        return Redirect(redirectUrl);
    }
}
