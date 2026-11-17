using System;

namespace Task1_1.ImplicitInterfaces
{
    public interface IShape
    {
        string Name { get; }
        double GetArea();
        double GetPerimeter();
        void DisplayInfo();
    }

    public interface IResizable
    {
        void Resize(double factor);
    }

    public class Circle : IShape, IResizable
    {
        private double _radius;

        public Circle(double radius)
        {
            if (radius <= 0) throw new ArgumentException("Радіус має бути додатним");
            _radius = radius;
        }

        public string Name => "Коло";
        public double GetArea() => Math.PI * _radius * _radius;
        public double GetPerimeter() => 2 * Math.PI * _radius;

        public void DisplayInfo()
        {
            Console.WriteLine($"{Name}: R = {_radius:F2}, Площа = {GetArea():F2}, Периметр = {GetPerimeter():F2}");
        }

        public void Resize(double factor)
        {
            if (factor <= 0) throw new ArgumentException("Коефіцієнт має бути додатним");
            _radius *= factor;
        }
    }

    public class Rectangle : IShape, IResizable
    {
        private double _width;
        private double _height;

        public Rectangle(double width, double height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("Сторони мають бути додатними");
            _width = width;
            _height = height;
        }

        public string Name => "Прямокутник";
        public double GetArea() => _width * _height;
        public double GetPerimeter() => 2 * (_width + _height);

        public void DisplayInfo()
        {
            Console.WriteLine($"{Name}: {_width:F2}x{_height:F2}, Площа = {GetArea():F2}, Периметр = {GetPerimeter():F2}");
        }

        public void Resize(double factor)
        {
            if (factor <= 0) throw new ArgumentException("Коефіцієнт має бути додатним");
            _width *= factor;
            _height *= factor;
        }
    }

    public class Triangle : IShape
    {
        private double _a, _b, _c;

        public Triangle(double a, double b, double c)
        {
            if (a + b <= c || a + c <= b || b + c <= a)
                throw new ArgumentException("Сторони не утворюють трикутник");
            _a = a; _b = b; _c = c;
        }

        public string Name => "Трикутник";

        public double GetArea()
        {
            double s = (_a + _b + _c) / 2.0;
            return Math.Sqrt(s * (s - _a) * (s - _b) * (s - _c));
        }

        public double GetPerimeter() => _a + _b + _c;

        public void DisplayInfo()
        {
            Console.WriteLine($"{Name}: ({_a:F2}; {_b:F2}; {_c:F2}), Площа = {GetArea():F2}, Периметр = {GetPerimeter():F2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IShape[] shapes =
            {
                new Circle(5.0),
                new Rectangle(3.0, 4.0),
                new Triangle(3, 4, 5),
                new Circle(2.5),
                new Rectangle(6.0, 2.0)
            };

            Console.WriteLine("=== Поліморфізм через IShape ===");
            double totalArea = 0;
            foreach (IShape shape in shapes)
            {
                shape.DisplayInfo();
                totalArea += shape.GetArea();
            }
            Console.WriteLine($"\nЗагальна площа всіх фігур: {totalArea:F2}");

            Console.WriteLine("\n=== Масштабування (IResizable) ===");
            foreach (IShape shape in shapes)
            {
                if (shape is IResizable resizable)
                {
                    Console.Write("До: ");
                    shape.DisplayInfo();
                    resizable.Resize(2.0);
                    Console.Write("Після (x2): ");
                    shape.DisplayInfo();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{shape.Name} — не підтримує масштабування");
                }
            }
        }
    }
}