namespace Library.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? BorrowRequestByEmail { get; set; }
    public string? BorrowedByEmail { get; set; }
}
