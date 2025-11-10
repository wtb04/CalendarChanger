using CalendarChanger.Attributes;
using CalendarChanger.Infrastructure;
using CalendarChanger.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalendarChanger.Controllers.WebEditor
{
    [Authorized]
    public class RoosterModifierController : Controller
    {
        private readonly CalendarContext _db;

        public RoosterModifierController(CalendarContext db)
        {
            _db = db;
        }

        [HttpPost("/RoosterModifier/Add")]
        public async Task<IActionResult> Add(string url, string description)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest("URL required.");

            using var http = new HttpClient();
            try
            {
                var resp = await http.GetAsync(url.Trim());
                if (!resp.IsSuccessStatusCode)
                    return BadRequest("URL not reachable.");

                if (!resp.Content.Headers.ContentType?.MediaType?.Contains("text/calendar") == true)
                    return BadRequest("Not a valid calendar (.ics) URL.");
            }
            catch
            {
                return BadRequest("Invalid or unreachable URL.");
            }

            _db.RoosterUrls.Add(new RoosterUrl
            {
                Url = url.Trim(),
                Description = description?.Trim() ?? string.Empty
            });
            await _db.SaveChangesAsync();
            return Ok();
        }



        [HttpPost("/RoosterModifier/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.RoosterUrls.FindAsync(id);
            if (r == null) return NotFound();
            _db.RoosterUrls.Remove(r);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}