using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarChanger.Models;

public class CalendarFilter
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Column(TypeName = "TEXT")]
    public string[] IncludeTypes { get; set; } = [];
}