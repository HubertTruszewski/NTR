using System.ComponentModel.DataAnnotations;

namespace lab2.Models;

public class User
{
    public int userId { get; set; }
    [Required]
    public string? username { get; set; }
    [Required]
    public string? pwd { get; set; }
}