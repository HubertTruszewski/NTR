using System.ComponentModel.DataAnnotations;

namespace lab3.Models;

public class Book
{
  public int bookId { get; set; }
  [Required]
  public string? author { get; set; }
  [Required]
  public string? title { get; set; }
  [Required]
  public int date { get; set; }
  [Required]
  public string? publisher { get; set; }
  [ConcurrencyCheck]
  public User? user { get; private set; }
  [Required]
  [ConcurrencyCheck]
  public string? reserved { get; set; }
  [Required]
  [ConcurrencyCheck]
  public string? leased { get; set; }

  [Timestamp]
  public byte[]? rowVersion { get; set; }

  public bool IsLeased()
  {
    return string.CompareOrdinal(leased, DateTime.Now.ToString("yyyy-MM-dd")) >= 0;
  }
  
  public bool IsReserved()
  {
    return string.CompareOrdinal(reserved, DateTime.Now.ToString("yyyy-MM-dd")) >= 0 && !IsLeased();
  }

  public bool IsAvailable()
  {
    return !(IsReserved() || IsLeased());
  }

  public bool IsReservedForUser(string? loggedUser)
  {
    return IsReserved() && user!.username == loggedUser;
  }
  
  public bool IsLeasedForUser(string? loggedUser)
  {
    return IsLeased() && user!.username == loggedUser;
  }

  public void Reserve(User loggedUser)
  {
    user = loggedUser;
    reserved = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
  }

  public void Lease()
  {
    leased = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
    reserved = "";
  }

  public void CancelReservation()
  {
    user = null;
    reserved = "";
  }

  public void Return()
  {
    leased = "";
    user = null;
  }
  
}