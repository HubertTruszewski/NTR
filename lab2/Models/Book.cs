namespace lab2.Models;

public class Book
{
  public int id { get; set; }
  public string? author { get; set; }
  public string? title { get; set; }
  public int date { get; set; }
  public string? publisher { get; set; }
  public string? user { get; set; }
  public string? reserved { get; set; }
  public string? leased { get; set; }

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
    return IsReserved() && user == loggedUser;
  }
  
  public bool IsLeasedForUser(string? loggedUser)
  {
    return IsLeased() && user == loggedUser;
  }

  public void Reserve(string? loggedUser)
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
    user = "";
    reserved = "";
  }

  public void Return()
  {
    leased = "";
    user = "";
  }
  
}