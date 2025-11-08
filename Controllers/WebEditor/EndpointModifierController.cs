using CalendarChanger.Attributes;
using CalendarChanger.Infrastructure;
using CalendarChanger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Controllers.WebEditor
{
    [Authorized]
    public class EndpointModifierController(CalendarContext db) : Controller
    {
        [HttpPost("/EndpointModifier/Add")]
        public async Task<IActionResult> Add(string name, string types)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name required.");

            bool exists = await db.CalendarFilters
                .AnyAsync(f => f.Name.ToLower() == name.ToLower());
            if (exists)
                return Conflict("Endpoint already exists.");

            var filter = new CalendarFilter
            {
                Name = name.Trim(),
                IncludeTypes = types
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim())
                    .ToArray()
            };

            db.CalendarFilters.Add(filter);
            await db.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("/EndpointModifier/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, string name, string includeTypes)
        {
            var filter = await db.CalendarFilters.FindAsync(id);
            if (filter == null)
                return NotFound();

            filter.Name = name.Trim();
            filter.IncludeTypes = includeTypes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToArray();

            await db.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPost("/EndpointModifier/BulkEdit")]
        public async Task<IActionResult> BulkEdit([FromBody] List<EndpointUpdateDto> updates)
        {
            foreach (var u in updates)
            {
                var filter = await db.CalendarFilters.FindAsync(u.Id);
                if (filter == null) continue;
                filter.IncludeTypes = u.Types
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim())
                    .ToArray();
            }
            await db.SaveChangesAsync();
            return Ok();
        }

        public record EndpointUpdateDto(int Id, string Types);


        [HttpPost("/EndpointModifier/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var filter = await db.CalendarFilters.FindAsync(id);
            if (filter == null)
                return NotFound();

            db.CalendarFilters.Remove(filter);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
