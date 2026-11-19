using Library.Domain.Models;

namespace Library.Domain.Interfaces
{
    public interface IBorrowRecordRepository
    {
        void SaveRecord(BorrowRecord record);
        List<BorrowRecord> GetAllRecords();
        BorrowRecord? GetRecordById(int id);
        List<BorrowRecord> GetUserBorrows(int userId);
        List<BorrowRecord> GetActiveRecords();
        void UpdateRecord(BorrowRecord record);
        void SaveBorrow(BorrowRecord borrow);
        void DeleteAllBorrows();
        List<BorrowRecord> GetAllBorrows();
    }
}
