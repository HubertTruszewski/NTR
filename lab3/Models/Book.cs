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
  public User? user { get; private set; }
  public DateOnly? reserved { get; set; }
  public DateOnly? leased { get; set; }

  [Timestamp]
  public byte[]? rowVersion { get; set; }

  public bool IsLeased()
  {
    return leased >= DateOnly.FromDateTime(DateTime.Now);
  }
  
  public bool IsReserved()
  {
    return reserved >= DateOnly.FromDateTime(DateTime.Now) && !IsLeased();
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
    reserved = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
  }

  public void Lease()
  {
    leased = DateOnly.FromDateTime(DateTime.Now.AddDays(14));
    reserved = null;
  }

  public void CancelReservation()
  {
    user = null;
    reserved = null;
  }

  public void Return()
  {
    leased = null;
    user = null;
  }
  
}