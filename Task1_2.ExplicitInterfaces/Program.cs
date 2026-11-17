using System;

namespace Task1_2.ExplicitInterfaces
{
    public interface IScreenPrintable
    {
        void Print();
        string GetFormat();
    }

    public interface IPrinterPrintable
    {
        void Print();
        string GetFormat();
    }

    public class Report : IScreenPrintable, IPrinterPrintable
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Report(string title, string content)
        {
            Title = title;
            Content = content;
        }

        void IScreenPrintable.Print() => Console.WriteLine($"[ЕКРАН] Звіт: {Title}\n{Content}");
        string IScreenPrintable.GetFormat() => "HTML";

        void IPrinterPrintable.Print() => Console.WriteLine($"[ПРИНТЕР] Друк звіту: {Title} ({Content.Length} символів)");
        string IPrinterPrintable.GetFormat() => "PDF";

        public void ShowSummary() => Console.WriteLine($"Звіт: {Title} ({Content.Length} символів)");
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Report report = new Report("Фінансовий звіт",
                "Дохід за квартал: 150 000 грн. Витрати: 95 000 грн.");

            report.ShowSummary();

            IScreenPrintable screen = report;
            Console.WriteLine($"\nФормат екрану: {screen.GetFormat()}");
            screen.Print();

            IPrinterPrintable printer = report;
            Console.WriteLine($"\nФормат принтера: {printer.GetFormat()}");
            printer.Print();

            Console.WriteLine("\n=== Через приведення типу ===");
            ((IScreenPrintable)report).Print();
            ((IPrinterPrintable)report).Print();

            // report.Print(); // Помилка компіляції CS1061
        }
    }
}