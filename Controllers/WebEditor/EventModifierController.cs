using CalendarChanger.Attributes;
using CalendarChanger.Infrastructure;
using CalendarChanger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Controllers.WebEditor;

public class EventModifierController : Controller
{
    private readonly CalendarContext _db;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public EventModifierController(CalendarContext db, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("/EventModifier/Login")]
    public IActionResult Login(string? id = null)
    {
        ViewBag.Id = id;
        return View();
    }

    [HttpPost("/EventModifier/Login")]
    public IActionResult LoginPost(string key, string? id)
    {
        if (key != _config["ApiKey"])
            return Content("Invalid key");

        HttpContext.Session.SetString("ApiKey", key);
        return Redirect(id == null ? "/EventModifier/Index" : $"/EventModifier/Edit/{id}");
    }

    [Authorized]
    public async Task<IActionResult> Index()
    {
        var items = await _db.Events.AsNoTracking()
            .OrderByDescending(e => e.Id)
            .ToListAsync();
        return View(items);
    }

    [Authorized]
    [HttpGet("/EventModifier/Edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        var e = await _db.Events.FindAsync(id) ?? new EventMeta { Id = id };
        return View(e);
    }

    [Authorized]
    [HttpPost("/EventModifier/Edit/{id}")]
    public async Task<IActionResult> EditPost(string id, EventMeta model)
    {
        var e = await _db.Events.FindAsync(id) ?? new EventMeta { Id = id };
        e.CustomTitle = model.CustomTitle;
        e.CustomStart = model.CustomStart;
        e.CustomEnd = model.CustomEnd;
        e.Hidden = model.Hidden;

        if (_db.Entry(e).State == EntityState.Detached)
            _db.Events.Add(e);

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorized]
    [HttpPost("/EventModifier/Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev != null)
        {
            _db.Events.Remove(ev);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorized]
    [HttpPost]
    public async Task<IActionResult> DeletePast()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var oldEvents = _db.Events
            .Where(e => e.Date != null && e.Date < today)
            .ToList();

        if (oldEvents.Count == 0)
            return Ok();

        _db.RemoveRange(oldEvents);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [Authorized]
    [HttpPost]
    public async Task<IActionResult> EnsureAndRefresh(string id)
    {
        var existing = await _db.FindAsync<EventMeta>(id);
        if (existing == null)
        {
            var ev = new EventMeta
            {
                Id = id,
                CustomTitle = null,
                CustomStart = null,
                CustomEnd = null,
                Hidden = false
            };
            _db.Add(ev);
            await _db.SaveChangesAsync();
        }

        await Refresh();
        return Ok();
    }

    [Authorized]
    [HttpPost("/EventModifier/Refresh")]
    public async Task<IActionResult> Refresh()
    {
        var settings = await _db.CalendarSettings.AsNoTracking().FirstOrDefaultAsync();
        if (settings == null || string.IsNullOrWhiteSpace(settings.BaseUrl))
            return BadRequest("Base URL not configured in database.");

        string baseUrl = settings.BaseUrl.TrimEnd('/');

        var filters = await _db.CalendarFilters.AsNoTracking().ToListAsync();
        if (filters.Count == 0)
            return BadRequest("No endpoints defined in database.");

        var client = _httpClientFactory.CreateClient();

        foreach (var f in filters)
        {
            var url = $"{baseUrl}/{f.Name}";
            try
            {
                var res = await client.GetAsync(url);
                res.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh failed for {url}: {ex.Message}");
            }
        }

        return Ok();
    }

}
