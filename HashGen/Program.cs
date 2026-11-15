using BCrypt.Net;

string password = "paata2011";
int workFactor = 11;
string hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor);
Console.WriteLine(hash);
