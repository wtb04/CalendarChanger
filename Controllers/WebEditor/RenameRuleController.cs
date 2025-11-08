using CalendarChanger.Attributes;
using CalendarChanger.Infrastructure;
using CalendarChanger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarChanger.Controllers.WebEditor
{
    [Authorized]
    public class RenameRuleController : Controller
    {
        private readonly CalendarContext _db;

        public RenameRuleController(CalendarContext db)
        {
            _db = db;
        }

        [HttpPost("/RenameRule/Add")]
        public async Task<IActionResult> Add(string match, string replace)
        {
            if (string.IsNullOrWhiteSpace(match) || string.IsNullOrWhiteSpace(replace))
                return BadRequest("Both match and replace are required.");

            _db.RenameRules.Add(new RenameRule
            {
                Match = match.Trim(),
                Replace = replace.Trim()
            });

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/RenameRule/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _db.RenameRules.FindAsync(id);
            if (rule == null)
                return NotFound();

            _db.RenameRules.Remove(rule);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("/RenameRule/List")]
        public async Task<IActionResult> List()
        {
            var rules = await _db.RenameRules
                .OrderBy(r => r.Match)
                .ToListAsync();
            return Json(rules);
        }
    }
}