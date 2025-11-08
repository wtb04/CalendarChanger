using CalendarChanger.Attributes;
using CalendarChanger.Infrastructure;
using CalendarChanger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Controllers.WebEditor
{
    [Authorized]
    public class SettingsController : Controller
    {
        private readonly CalendarContext _db;

        public SettingsController(CalendarContext db)
        {
            _db = db;
        }

        [HttpGet("/Settings")]
        public async Task<IActionResult> Index()
        {
            var endpoints = await _db.CalendarFilters.OrderBy(f => f.Name).ToListAsync();
            var baseUrl = await _db.CalendarSettings.FirstOrDefaultAsync();
            var roosterUrls = await _db.RoosterUrls.ToListAsync();
            var renameRules = await _db.RenameRules.ToListAsync();

            var model = new SettingsViewModel
            {
                CalendarEndpoints = endpoints,
                BaseUrl = baseUrl?.BaseUrl ?? "",
                RoosterUrls = roosterUrls,
                RenameRules = renameRules,
            };
            return View(model);
        }

        [HttpPost("/Settings/SaveBaseUrl")]
        public async Task<IActionResult> SaveBaseUrl(string baseUrl)
        {
            var settings = await _db.CalendarSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new CalendarSettings();
                _db.CalendarSettings.Add(settings);
            }
            settings.BaseUrl = baseUrl.Trim();
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
    
    

    public class SettingsViewModel
    {
        public List<CalendarFilter> CalendarEndpoints { get; set; } = new();
        public string BaseUrl { get; set; } = "";
        public List<RoosterUrl> RoosterUrls { get; set; } = new();
        
        public List<RenameRule> RenameRules { get; set; } = new();
    }
}