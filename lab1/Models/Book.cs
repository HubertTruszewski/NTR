namespace lab1.Models;

public class Book {
  public int id { get; set; }
  public string? author { get; set; }
  public string? title { get; set; }
  public int date{ get; set; }
  public string? publisher { get; set; }
  public string? user { get; set; }
  public string? reserved { get; set; }
  public string? leased { get; set; }
}