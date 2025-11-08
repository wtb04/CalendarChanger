using CalendarChanger.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using CalendarChanger.Services;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Controllers
{
    [ApiController]
    [Route("")]
    public class CalendarController(ICalendarService calendarService, CalendarContext db) : ControllerBase
    {
        [HttpGet("{filterName}")]
        public async Task<IActionResult> GetFiltered(string filterName)
        {
            var filter = await db.CalendarFilters
                .FirstOrDefaultAsync(f => f.Name.ToLower() == filterName.ToLower());


            if (filter == null)
                return NotFound();

            var calendarData = await calendarService.GetCalendarAsync(
                filter.Name,
                includeTypes: filter.IncludeTypes
            );

            return File(calendarData, "text/calendar", $"{filter.Name}.ics");
        }

        [HttpGet("other")]
        public async Task<IActionResult> GetOther()
        {
            var allTypes = db.CalendarFilters
                .AsEnumerable()
                .SelectMany(f => f.IncludeTypes)
                .Distinct()
                .ToArray();


            var calendarData = await calendarService.GetCalendarAsync(
                "Other",
                excludeTypes: allTypes
            );

            return File(calendarData, "text/calendar", "other.ics");
        }
    }
}
