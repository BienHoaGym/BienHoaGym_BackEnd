using System;
using Microsoft.AspNetCore.Identity;

public class User { }

public class Program
{
    public static void Main()
    {
        var hasher = new PasswordHasher<User>();
        var hash = hasher.HashPassword(new User(), "123456");
        Console.WriteLine("HASH_START");
        Console.WriteLine(hash);
        Console.WriteLine("HASH_END");
    }
}
