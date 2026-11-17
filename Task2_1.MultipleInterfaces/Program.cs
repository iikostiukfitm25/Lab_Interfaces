using System;
using System.Collections.Generic;

namespace Task2_1.MultipleInterfaces
{
    public interface ISerializable { string Serialize(); }
    public interface IValidatable { bool IsValid(); List<string> GetErrors(); }
    public interface ILoggable { void Log(string action); List<string> GetLogHistory(); }
    public interface IPersistable : ISerializable, IValidatable
    {
        void Save();
        void Load(string data);
    }

    public class User : IPersistable, ILoggable
    {
        private readonly List<string> _logHistory = new();

        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        public User(string username, string email, int age)
        {
            Username = username;
            Email = email;
            Age = age;
            Log("Створено користувача");
        }

        public string Serialize() => $"{{\"username\":\"{Username}\", \"email\":\"{Email}\", \"age\":{Age}}}";

        public bool IsValid() => GetErrors().Count == 0;

        public List<string> GetErrors()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Username)) errors.Add("Ім'я користувача не може бути порожнім");
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@')) errors.Add("Некоректний формат Email");
            if (Age < 18) errors.Add("Вік користувача має бути не менше 18 років");
            return errors;
        }

        public void Save()
        {
            if (IsValid())
            {
                Log("Успішне збереження");
                Console.WriteLine($"Збережено: {Serialize()}");
            }
            else
            {
                Log("Відмова збереження: помилки валідації");
                Console.WriteLine($"Помилка збереження User ({Username}): {string.Join("; ", GetErrors())}");
            }
        }

        public void Load(string data)
        {
            var parts = data.Split(';');
            if (parts.Length == 3)
            {
                Username = parts[0];
                Email = parts[1];
                if (int.TryParse(parts[2], out int parsedAge)) Age = parsedAge;
                Log("Дані завантажено");
            }
        }

        public void Log(string action) => _logHistory.Add($"[{DateTime.Now:HH:mm:ss}] {action}");
        public List<string> GetLogHistory() => new(_logHistory);
        public override string ToString() => $"User: {Username}, Email: {Email}, Age: {Age}";
    }

    public class Order : IPersistable, ILoggable
    {
        private readonly List<string> _logHistory = new();

        public int OrderId { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }

        public Order(int orderId, string product, decimal amount)
        {
            OrderId = orderId;
            Product = product;
            Amount = amount;
            Log($"Створено замовлення #{orderId}");
        }

        public string Serialize() => $"{{\"orderId\":{OrderId}, \"product\":\"{Product}\", \"amount\":{Amount:F2}}}";

        public bool IsValid() => GetErrors().Count == 0;

        public List<string> GetErrors()
        {
            var errors = new List<string>();
            if (OrderId <= 0) errors.Add("Номер замовлення має бути додатним");
            if (string.IsNullOrWhiteSpace(Product)) errors.Add("Назва товару не може бути порожньою");
            if (Amount <= 0) errors.Add("Сума замовлення має бути більшою за 0");
            return errors;
        }

        public void Save()
        {
            if (IsValid())
            {
                Log("Замовлення збережено");
                Console.WriteLine($"Збережено: {Serialize()}");
            }
            else
            {
                Log("Помилка валідації замовлення");
                Console.WriteLine($"Помилка збереження Order #{OrderId}: {string.Join("; ", GetErrors())}");
            }
        }

        public void Load(string data)
        {
            var parts = data.Split(';');
            if (parts.Length == 3 && int.TryParse(parts[0], out int id) && decimal.TryParse(parts[2], out decimal amt))
            {
                OrderId = id;
                Product = parts[1];
                Amount = amt;
                Log("Замовлення завантажено");
            }
        }

        public void Log(string action) => _logHistory.Add($"[{DateTime.Now:HH:mm:ss}] {action}");
        public List<string> GetLogHistory() => new(_logHistory);
        public override string ToString() => $"Order #{OrderId}: {Product}, Amount: {Amount:F2} грн";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            User user1 = new User("admin", "admin@example.com", 25);
            User user2 = new User("", "invalid-email", 15);
            Order order = new Order(1, "Ноутбук", 25000m);

            Console.WriteLine("=== Валідація (IValidatable) ===");
            IValidatable[] validatables = { user1, user2, order };
            foreach (var v in validatables)
            {
                Console.WriteLine($"{v}: Valid = {v.IsValid()}");
                if (!v.IsValid())
                {
                    foreach (string err in v.GetErrors())
                        Console.WriteLine($"  Помилка: {err}");
                }
            }

            Console.WriteLine("\n=== Серіалізація (ISerializable) ===");
            ISerializable[] serializables = { user1, order };
            foreach (var s in serializables)
                Console.WriteLine(s.Serialize());

            Console.WriteLine("\n=== Збереження (IPersistable) ===");
            IPersistable[] persistables = { user1, user2, order };
            foreach (var p in persistables)
                p.Save();

            Console.WriteLine("\n=== Історія логів (ILoggable) ===");
            user1.Log("Спроба повторного збереження");
            foreach (string log in user1.GetLogHistory())
                Console.WriteLine($"  {log}");
        }
    }
}