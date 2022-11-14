namespace lab3.Models;

public class BookActionModel
{
    public string? user { get; set; }
    public int book { get; set; }
    public byte[]? rowVersion { get; set; }
}