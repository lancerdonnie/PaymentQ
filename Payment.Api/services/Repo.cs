using System;
using System.Collections.Generic;

namespace Payment.Api.Repository
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public Double Balance { get; set; }
    }
    public class User
    {
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Tuple<int, string> Bank { get; set; }
    }
    public static class Repo
    {
        public static List<User> users = new List<User> {
            new User { Account = new Account { AccountNumber = "0000001", Balance = 1_000_000 }, Name = "Christiano Ronaldo", Email = "ronaldo@email.com", Password = "ronaldo", Phone = "080000001" },
            new User { Account = new Account { AccountNumber = "0000002", Balance = 2_000_000 }, Name = "Lionel Messi", Email = "messi@email.com", Password = "messi", Phone = "080000002"},
            new User { Account = new Account { AccountNumber = "0000003", Balance = 5_000 }, Name = "Romelu Lukaku", Email = "lukaku@email.com", Password = "lukaku", Phone = "080000003" },
            new User { Account = new Account { AccountNumber = "0000004", Balance = 0 }, Name = "Bruno Fernandes", Email = "bruno@email.com", Password = "bruno", Phone = "080000004"},
            new User { Account = new Account { AccountNumber = "0000005", Balance = 15_000_000 }, Name = "Mohammed Salah", Email = "salah@email.com", Password = "salah", Phone = "080000005"}
        };
    }
}