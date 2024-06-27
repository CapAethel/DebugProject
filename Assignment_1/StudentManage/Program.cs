using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        DisplayStudents(students, "All Students:");
                        break;
                    case 2:
                        AddStudent(students);
                        break;
                    case 3:
                        DeleteStudent(students);
                        break;
                    case 4:
                        DisplayMaleStudent(students);
                        break;
                    case 5:
                        OldestStudent(students);
                        break;
                    case 6:
                        DisplayFullNames(students);
                        break;
                    case 7:
                        Student2000(students);
                        break;
                    case 8:
                        StudentLarger2000(students);
                        break;
                    case 9:
                        StudentSmaller2000(students);
                        break;
                    case 10:
                        BornInHanoi(students);
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
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1))
                {
                    var values = line.Split(',');

                    try
                    {
                        students.Add(new Student
                        {
                            ID = int.Parse(values[0]),
                            LastName = values[1],
                            FirstName = values[2],
                            DateOfBirth = DateTime.ParseExact(values[3], dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None),
                            Gender = values[4],
                            PlaceOfBirth = values[5],
                            Mobile = values[6],
                            IsGraduated = bool.Parse(values[7])
                        });
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing student data: {line}. Error: {ex.Message}");
                    }
                }
            }

            return students;
        }

        static void DisplayStudents(List<Student> students, string header)
        {
            Console.WriteLine(header);
            Console.WriteLine("{0,-8} {1,-16} {2,-19} {3,-15} {4,-6} {5,-15} {6,-15} {7,-9} {8,-5}", "ID", "Last Name", "First Name", "Date of Birth", "Gender", "Place of Birth", "Mobile", "Graduated", "Age");
            foreach (var student in students)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-15:yyyy-MM-dd} {4,-6} {5,-15} {6,-15} {7,-9} {8,-5}", 
                    student.ID, student.LastName, student.FirstName, student.DateOfBirth, student.Gender, student.PlaceOfBirth, student.Mobile, student.IsGraduated ? "Yes" : "No", student.Age);
            }
        }

        static void DeleteStudent(List<Student> students)
        {
            Console.Write("Enter ID of the student to delete: ");
            int id = int.Parse(Console.ReadLine());
            Student studentToRemove = null;
            foreach (var student in students)
            {
                if (student.ID == id)
                {
                    studentToRemove = student;
                    break;
                }
            }

            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                Console.WriteLine($"Student with ID {id} has been deleted.");
            }
            else
            {
                Console.WriteLine($"Student with ID {id} not found.");
            }
        }

        static void DisplayMaleStudent(List<Student> students)
        {
            List<Student> maleStudents = new List<Student>();
            foreach (var student in students)
            {
                if (student.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                {
                    maleStudents.Add(student);
                }
            }
            DisplayStudents(maleStudents, "Male Students:");
        }

        static void OldestStudent(List<Student> students)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No students available.");
                return;
            }

            Student oldestStudent = students[0];
            foreach (var student in students)
            {
                if (student.DateOfBirth < oldestStudent.DateOfBirth)
                {
                    oldestStudent = student;
                }
            }
            DisplayStudents(new List<Student> { oldestStudent }, "Oldest Student:");
        }

        static void DisplayFullNames(List<Student> students)
        {
            Console.WriteLine("Full Names:");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.LastName} {student.FirstName}");
            }
        }

        static void Student2000(List<Student> students)
        {
            List<Student> bornIn2000 = new List<Student>();
            foreach (var student in students)
            {
                if (student.DateOfBirth.Year == 2000)
                {
                    bornIn2000.Add(student);
                }
            }
            DisplayStudents(bornIn2000, "Students Born in 2000:");
        }

        static void StudentLarger2000(List<Student> students)
        {
            List<Student> bornAfter2000 = new List<Student>();
            foreach (var student in students)
            {
                if (student.DateOfBirth.Year > 2000)
                {
                    bornAfter2000.Add(student);
                }
            }
            DisplayStudents(bornAfter2000, "Students Born After 2000:");
        }

        static void StudentSmaller2000(List<Student> students)
        {
            List<Student> bornBefore2000 = new List<Student>();
            foreach (var student in students)
            {
                if (student.DateOfBirth.Year < 2000)
                {
                    bornBefore2000.Add(student);
                }
            }
            DisplayStudents(bornBefore2000, "Students Born Before 2000:");
        }

        static void BornInHanoi(List<Student> students)
        {
            foreach (var student in students)
            {
                if (student.PlaceOfBirth.Equals("Ha Noi", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayStudents(new List<Student> { student }, "First Student Born in Ha Noi:");
                    return;
                }
            }
            Console.WriteLine("No student born in Ha Noi found.");
        }

        static void AddStudent(List<Student> students)
        {
            Student newStudent = new Student();

            Console.Write("Enter ID: ");
            newStudent.ID = int.Parse(Console.ReadLine());
            Console.Write("Enter Last Name: ");
            newStudent.LastName = Console.ReadLine();
            Console.Write("Enter First Name: ");
            newStudent.FirstName = Console.ReadLine();
            Console.Write("Enter Date of Birth (yyyy-MM-dd or dd/MM/yyyy): ");
            newStudent.DateOfBirth = DateTime.ParseExact(Console.ReadLine(), new[] { "yyyy-MM-dd", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
            Console.Write("Enter Gender: ");
            newStudent.Gender = Console.ReadLine();
            Console.Write("Enter Place of Birth: ");
            newStudent.PlaceOfBirth = Console.ReadLine();
            Console.Write("Enter Mobile: ");
            newStudent.Mobile = Console.ReadLine();
            Console.Write("Is Graduated (true/false): ");
            newStudent.IsGraduated = bool.Parse(Console.ReadLine());

            students.Add(newStudent);
        }

        static void SaveStudents(string filePath, List<Student> students)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("ID,LastName,FirstName,DateOfBirth,Gender,PlaceOfBirth,Mobile,IsGraduated");
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.ID},{student.LastName},{student.FirstName},{student.DateOfBirth:yyyy-MM-dd},{student.Gender},{student.PlaceOfBirth},{student.Mobile},{student.IsGraduated}");
                }
            }
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
