using Library.Domain.Models;
using Library.Domain.Interfaces;
using System.IO;

namespace Library.Infrastructure.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly string _path;

        public BorrowRecordRepository()
        {
            var dataFolderPath = Path.Combine(AppContext.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            _path = Path.Combine(dataFolderPath, "Borrows.txt");

            if (!File.Exists(_path))
                File.Create(_path).Dispose();
        }

        public void SaveRecord(BorrowRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            try
            {
                File.AppendAllLines(_path, new List<string> { record.ToString() });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save borrow record to {_path}", ex);
            }
        }

        public List<BorrowRecord> GetAllRecords()
        {
            var recordList = new List<BorrowRecord>();

            try
            {
                if (!File.Exists(_path))
                    return recordList;

                var recordLines = File.ReadAllLines(_path);

                if (recordLines.Length == 0)
                    return recordList;

                foreach (var line in recordLines)
                {
                    var record = BorrowRecord.Parse(line);
                    if (record != null)
                        recordList.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read borrow records from {_path}", ex);
            }

            return recordList;
        }

        public BorrowRecord? GetRecordById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Record ID must be positive.", nameof(id));

            try
            {
                var records = GetAllRecords();
                return records.FirstOrDefault(r => r.Id == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read borrow records from {_path}", ex);
            }
        }

        public List<BorrowRecord> GetUserBorrows(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be positive.", nameof(userId));

            try
            {
                var records = GetAllRecords();
                return records.Where(r => r.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read borrow records from {_path}", ex);
            }
        }

        public List<BorrowRecord> GetActiveRecords()
        {
            try
            {
                var records = GetAllRecords();
                return records.Where(r => r.Status == "Approved" && !r.ReturnDate.HasValue).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read borrow records from {_path}", ex);
            }
        }

        public void UpdateRecord(BorrowRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            try
            {
                var records = GetAllRecords();
                var recordToUpdate = records.FirstOrDefault(r => r.Id == record.Id);

                if (recordToUpdate == null)
                    throw new InvalidOperationException($"Borrow record with ID {record.Id} not found.");

                records.Remove(recordToUpdate);
                records.Add(record);

                File.WriteAllText(_path, "");
                foreach (var r in records)
                {
                    File.AppendAllLines(_path, new List<string> { r.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update borrow record in {_path}", ex);
            }
        }

        public void SaveBorrow(BorrowRecord borrow)
        {
            if (borrow == null)
                throw new ArgumentNullException(nameof(borrow));

            try
            {
                var records = GetAllRecords();
                var existingRecord = records.FirstOrDefault(r => r.Id == borrow.Id);

                if (existingRecord != null)
                {
                    UpdateRecord(borrow);
                }
                else
                {
                    File.AppendAllLines(_path, new List<string> { borrow.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save borrow record to {_path}", ex);
            }
        }

        public void DeleteAllBorrows()
        {
            try
            {
                File.WriteAllText(_path, "");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to clear borrow records from {_path}", ex);
            }
        }

        public List<BorrowRecord> GetAllBorrows()
        {
            return GetAllRecords();
        }
    }
}
