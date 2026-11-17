using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task3_1.StandardInterfaces
{
    public class Student : IComparable<Student>
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public double GPA { get; set; }

        public Student(string name, string group, double gpa)
        {
            Name = name;
            Group = group;
            GPA = gpa;
        }

        public int CompareTo(Student? other)
        {
            if (other is null) return 1;
            return other.GPA.CompareTo(GPA);
        }

        public override string ToString() => $"{Name} ({Group}), GPA: {GPA:F2}";
    }

    public class StudentGroup : IEnumerable<Student>
    {
        private readonly List<Student> _students = new();
        public string GroupName { get; set; }

        public StudentGroup(string groupName) => GroupName = groupName;

        public void AddStudent(Student student) => _students.Add(student);
        public int Count => _students.Count;
        public Student this[int index] => _students[index];

        public IEnumerable<Student> GetHonorStudents(double threshold = 4.5)
        {
            foreach (Student student in _students)
            {
                if (student.GPA >= threshold)
                    yield return student;
            }
        }

        public IEnumerator<Student> GetEnumerator() => _students.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void SortByGPA() => _students.Sort();
        public double GetAverageGPA() => _students.Count == 0 ? 0 : _students.Average(s => s.GPA);
        public Student? GetTopStudent() => _students.OrderByDescending(s => s.GPA).FirstOrDefault();

        public void DisplayAll()
        {
            Console.WriteLine($"=== Група {GroupName} ({Count} студентів) ===");
            foreach (Student s in this)
                Console.WriteLine($"  {s}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            StudentGroup group = new StudentGroup("КІ-21");
            group.AddStudent(new Student("Олена Петренко", "КІ-21", 4.8));
            group.AddStudent(new Student("Андрій Коваленко", "КІ-21", 3.5));
            group.AddStudent(new Student("Ірина Мельник", "КІ-21", 4.9));
            group.AddStudent(new Student("Максим Бондаренко", "КІ-21", 4.2));
            group.AddStudent(new Student("Наталія Шевченко", "КІ-21", 4.6));
            group.AddStudent(new Student("Дмитро Ткаченко", "КІ-21", 3.9));

            Console.WriteLine("=== До сортування ===");
            group.DisplayAll();

            Console.WriteLine("\n=== Після сортування за GPA ===");
            group.SortByGPA();
            group.DisplayAll();

            Console.WriteLine("\n=== Відмінники (GPA >= 4.5) ===");
            foreach (Student s in group.GetHonorStudents())
                Console.WriteLine($"  {s}");

            Console.WriteLine($"\nСередній GPA: {group.GetAverageGPA():F2}");
            Console.WriteLine($"Найкращий студент: {group.GetTopStudent()}");

            Console.WriteLine("\n=== LINQ запити ===");
            var top3 = group.OrderByDescending(s => s.GPA).Take(3);
            Console.WriteLine("Топ-3 за GPA:");
            foreach (var s in top3)
                Console.WriteLine($"  {s}");

            var averageAbove4 = group.Where(s => s.GPA > 4.0).Average(s => s.GPA);
            Console.WriteLine($"Середній GPA серед тих, хто > 4.0: {averageAbove4:F2}");
        }
    }
}