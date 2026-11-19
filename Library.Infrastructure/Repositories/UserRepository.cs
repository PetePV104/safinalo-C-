using Library.Domain.Models;
using Library.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Library.Domain.Interfaces;
using System.IO;

namespace Library.Infrastructure.Repositories
{    
    public class UserRepository : IUserRepository
    {
        private readonly string _path;

        public UserRepository()
        {
            var dataFolderPath = Path.Combine(AppContext.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);
            _path = Path.Combine(dataFolderPath, "UserData.txt");
            if (!File.Exists(_path))
                File.Create(_path).Dispose();
        }

        public void SaveUser(User user)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            var users = File.Exists(_path)
                ? File.ReadAllLines(_path).ToList()
                : new List<string>();
            var existingIndex = users.FindIndex(line =>
            {
                var parts = line.Split('|');
                return parts.Length >= 7 && int.TryParse(parts[0], out var id) && id == user.Id;
            });

            if (existingIndex >= 0)
            {
                users[existingIndex] = user.ToString();
                File.WriteAllLines(_path, users);
                return;
            }

            File.AppendAllLines(_path, new[] { user.ToString() });
        }
        public void SaveUsers(List<string> users)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllLines(_path, users);
        }
        public void DeleteAllUsers()
        {
            File.WriteAllText(_path, ""); 
		}
        public List<User> GetAllUsers()
        {
            if (!File.Exists(_path))
                return new List<User>();

            var userlines = File.ReadAllLines(_path);
            List<User> userList = new List<User>();

            foreach (var item in userlines)
            {
             var parts = item.Split('|');
                if (parts.Length < 7)
                    continue;
                User user = new User();
                user.Id = int.Parse(parts[0]);
                user.Name = parts[1];
                user.Email = parts[2];
                user.Password = parts[3];
                user.VerifiCationCode = parts[4];
                user.IsVerified = bool.Parse(parts[5]);
                user.Role = parts[6];
                userList.Add(user);
            }
            return userList;
        }
        public User? GetUserByEmail(string email)
        {
            if (!File.Exists(_path))
                return null;

            var userLines = File.ReadAllLines(_path).ToList();

            foreach (var item in userLines)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var parts = item.Split("|");
                if (parts.Length < 7 || parts[2] != email)
                    continue;

                User user = new User();
                user.Id = int.Parse(parts[0]);
                user.Name = parts[1];
                user.Email = parts[2];
                user.Password = parts[3];
                user.VerifiCationCode = parts[4];
                user.IsVerified = bool.Parse(parts[5]);
                user.Role = parts[6];

                return user;
            }
            return null;
        }
    }
}
