using Library.Domain.Models;
using Library.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain.Interfaces;
using System.IO;

namespace Library.Infrastructure.Repositories
{    
    public class UserRepository : IUserRepository
    {
        private readonly string _path;

        public UserRepository(string dataFolderPath)
        {
            if (string.IsNullOrWhiteSpace(dataFolderPath))
                throw new ArgumentException("Data folder path cannot be null or empty.", nameof(dataFolderPath));

            // Create directory if it doesn't exist
            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            _path = Path.Combine(dataFolderPath, "UserData.txt");

            // Create file if it doesn't exist
            if (!File.Exists(_path))
                File.Create(_path).Dispose();
        }

        public void SaveUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                File.AppendAllLines(_path, new List<string> { user.ToString() });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save user to {_path}", ex);
            }
        }

        public void SaveUsers(List<string> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            try
            {
                File.AppendAllLines(_path, users);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save users to {_path}", ex);
            }
        }

        public void DeleteAllUsers()
        {
            try
            {
                File.WriteAllText(_path, "");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete users from {_path}", ex);
            }
        }

        public List<User> GetAllUsers()
        {
            var userList = new List<User>();

            try
            {
                if (!File.Exists(_path))
                    return userList;

                var userlines = File.ReadAllLines(_path);

                if (userlines.Length == 0)
                    return userList;

                foreach (var item in userlines)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        continue;

                    var parts = item.Split('|');
                    if (parts.Length < 7)
                        continue;

                    User user = new User();
                    user.Id = int.Parse(parts[0]);
                    user.Name = parts[1];
                    user.Email = parts[2];
                    user.Password = parts[3];
                    user.VerificationCode = parts[4];
                    user.IsVerified = bool.Parse(parts[5]);
                    user.Role = parts[6];
                    userList.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read users from {_path}", ex);
            }

            return userList;
        }

        public User? GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            try
            {
                if (!File.Exists(_path))
                    return null;

                var userLines = File.ReadAllLines(_path);

                foreach (var item in userLines)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        continue;

                    var parts = item.Split('|');
                    if (parts.Length < 7)
                        continue;

                    if (parts[2] == email)
                    {
                        User user = new User();
                        user.Id = int.Parse(parts[0]);
                        user.Name = parts[1];
                        user.Email = parts[2];
                        user.Password = parts[3];
                        user.VerificationCode = parts[4];
                        user.IsVerified = bool.Parse(parts[5]);
                        user.Role = parts[6];

                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read users from {_path}", ex);
            }

            return null;
        }
    }
}
