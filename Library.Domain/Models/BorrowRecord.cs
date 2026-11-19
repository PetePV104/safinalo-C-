namespace Library.Domain.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected", etc.
        public string? Notes { get; set; }

        public override string ToString()
        {
            return $"{Id}|{UserId}|{BookId}|{BorrowDate:O}|{ReturnDate:O}|{Status}|{Notes}";
        }

        public static BorrowRecord? Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            try
            {
                var parts = line.Split('|');
                if (parts.Length < 7)
                    return null;

                return new BorrowRecord
                {
                    Id = int.Parse(parts[0]),
                    UserId = int.Parse(parts[1]),
                    BookId = int.Parse(parts[2]),
                    BorrowDate = DateTime.Parse(parts[3]),
                    ReturnDate = string.IsNullOrEmpty(parts[4]) ? null : DateTime.Parse(parts[4]),
                    Status = parts[5],
                    Notes = parts[6]
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
