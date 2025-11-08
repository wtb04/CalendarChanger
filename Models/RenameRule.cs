using System.ComponentModel.DataAnnotations;

namespace CalendarChanger.Models;

public class RenameRule
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Match { get; set; } = string.Empty;
    
    [Required]
    public string Replace { get; set; } = string.Empty;
}