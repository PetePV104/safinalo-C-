using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
        public string Role { get; set; } = "User";

        public override string ToString()
        {
            return $"{Id}|{Name}|{Email}|{Password}|{VerificationCode}|{IsVerified}|{Role}";
        }
    } 
      
}