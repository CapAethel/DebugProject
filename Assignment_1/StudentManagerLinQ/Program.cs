using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StudentManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "D:\\DOTnet\\Assignment_1\\student.csv";
            List<Student> students = LoadStudents(filePath);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Student Management System");
                Console.WriteLine("1. Display Students");
                Console.WriteLine("2. Add Student");
                Console.WriteLine("3. Delete Student using ID");
                Console.WriteLine("4. Display Male Students");
                Console.WriteLine("5. Display Oldest Students");
                Console.WriteLine("6. Display Full Names");
                Console.WriteLine("7. Display students whose birth year is 2000");
                Console.WriteLine("8. Display students whose birth year is after 2000");
                Console.WriteLine("9. Display students whose birth year is before 2000");
                Console.WriteLine("10. Display the first student born in Hanoi");
                Console.WriteLine("11. Save and Exit");
                Console.Write("Choose an option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        DisplayStudents(students);
                        break;
                    case 2:
                        AddStudent(students);
                        break;
                    case 3:
                        DeleteStudent(students);
                        break;
                    case 4:
                        DisplayMaleStudents(students);
                        break;
                    case 5:
                        DisplayOldestStudent(students);
                        break;
                    case 6:
                        DisplayFullNames(students);
                        break;
                    case 7:
                        DisplayStudentsBornInYear(students, 2000);
                        break;
                    case 8:
                        DisplayStudentsBornAfterYear(students, 2000);
                        break;
                    case 9:
                        DisplayStudentsBornBeforeYear(students, 2000);
                        break;
                    case 10:
                        DisplayFirstStudentBornInHanoi(students);
                        break;
                    case 11:
                        SaveStudents(filePath, students);
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static List<Student> LoadStudents(string filePath)
        {
            List<Student> students = new List<Student>();
            string[] dateFormats = { "yyyy-MM-dd", "dd/MM/yyyy" };

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).Skip(1);
                students = lines.Select(line =>
                {
                    var values = line.Split(',');
                    return new Student
                    {
                        ID = int.Parse(values[0]),
                        LastName = values[1],
                        FirstName = values[2],
                        DateOfBirth = DateTime.ParseExact(values[3], dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Gender = values[4],
                        PlaceOfBirth = values[5],
                        Mobile = values[6],
                        IsGraduated = bool.Parse(values[7])
                    };
                }).ToList();
            }

            return students;
        }

        static void DisplayStudents(List<Student> students)
        {
            Console.WriteLine("{0,-8} {1,-16} {2,-19} {3,-15} {4,-6} {5,-15} {6,-15} {7,-9} {8,-5}", "ID", "Last Name", "First Name", "Date of Birth", "Gender", "Place of Birth", "Mobile", "Graduated", "Age");
            students.ForEach(student => 
                Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-15:yyyy-MM-dd} {4,-6} {5,-15} {6,-15} {7,-9} {8,-5}", 
                    student.ID, student.LastName, student.FirstName, student.DateOfBirth, student.Gender, student.PlaceOfBirth, student.Mobile, student.IsGraduated ? "Yes" : "No", student.Age)
            );
        }

        static void DisplayMaleStudents(List<Student> students)
        {
            var maleStudents = students.Where(s => s.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase)).ToList();
            DisplayStudents(maleStudents);
        }

        static void DisplayOldestStudent(List<Student> students)
        {
            var oldestStudent = students.OrderBy(s => s.DateOfBirth).FirstOrDefault();
            if (oldestStudent != null)
                DisplayStudents(new List<Student> { oldestStudent });
            else
                Console.WriteLine("No students available.");
        }

        static void DisplayFullNames(List<Student> students)
        {
            var fullNames = students.Select(s => $"{s.LastName} {s.FirstName}").ToList();
            Console.WriteLine("Full Names:");
            fullNames.ForEach(Console.WriteLine);
        }

        static void DisplayStudentsBornInYear(List<Student> students, int year)
        {
            var bornInYear = students.Where(s => s.DateOfBirth.Year == year).ToList();
            DisplayStudents(bornInYear);
        }

        static void DisplayStudentsBornAfterYear(List<Student> students, int year)
        {
            var bornAfterYear = students.Where(s => s.DateOfBirth.Year > year).ToList();
            DisplayStudents(bornAfterYear);
        }

        static void DisplayStudentsBornBeforeYear(List<Student> students, int year)
        {
            var bornBeforeYear = students.Where(s => s.DateOfBirth.Year < year).ToList();
            DisplayStudents(bornBeforeYear);
        }

        static void DisplayFirstStudentBornInHanoi(List<Student> students)
        {
            var firstBornInHanoi = students.FirstOrDefault(s => s.PlaceOfBirth.Equals("Ha Noi", StringComparison.OrdinalIgnoreCase));
            if (firstBornInHanoi != null)
                DisplayStudents(new List<Student> { firstBornInHanoi });
            else
                Console.WriteLine("No student born in Ha Noi found.");
        }

        static void AddStudent(List<Student> students)
        {
            try
            {
                var newStudent = new Student
                {
                    ID = PromptForInt("Enter ID: "),
                    LastName = PromptForString("Enter Last Name: "),
                    FirstName = PromptForString("Enter First Name: "),
                    DateOfBirth = PromptForDate("Enter Date of Birth (yyyy-MM-dd or dd/MM/yyyy): "),
                    Gender = PromptForString("Enter Gender: "),
                    PlaceOfBirth = PromptForString("Enter Place of Birth: "),
                    Mobile = PromptForString("Enter Mobile: "),
                    IsGraduated = PromptForBool("Is Graduated (true/false): ")
                };

                students.Add(newStudent);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
            }
        }

        static void DeleteStudent(List<Student> students)
        {
            int id = PromptForInt("Enter ID of the student to delete: ");
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student != null)
            {
                students.Remove(student);
                Console.WriteLine($"Student with ID {id} has been deleted.");
            }
            else
            {
                Console.WriteLine($"Student with ID {id} not found.");
            }
        }

        static void SaveStudents(string filePath, List<Student> students)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("ID,LastName,FirstName,DateOfBirth,Gender,PlaceOfBirth,Mobile,IsGraduated");
                students.ForEach(student =>
                    writer.WriteLine($"{student.ID},{student.LastName},{student.FirstName},{student.DateOfBirth:yyyy-MM-dd},{student.Gender},{student.PlaceOfBirth},{student.Mobile},{student.IsGraduated}")
                );
            }
        }

        static int PromptForInt(string message)
        {
            Console.Write(message);
            return int.Parse(Console.ReadLine());
        }

        static string PromptForString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        static DateTime PromptForDate(string message)
        {
            Console.Write(message);
            return DateTime.ParseExact(Console.ReadLine(), new[] { "yyyy-MM-dd", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        static bool PromptForBool(string message)
        {
            Console.Write(message);
            return bool.Parse(Console.ReadLine());
        }
    }

    class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Mobile { get; set; }
        public bool IsGraduated { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}

