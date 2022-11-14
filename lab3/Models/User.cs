using System.ComponentModel.DataAnnotations;

namespace lab3.Models;

public class User
{
    public int userId { get; set; }
    [Required]
    public string? username { get; set; }
    [Required]
    public string? pwd { get; set; }

    [Timestamp]
    public byte[]? rowVersion { get; set; }
}