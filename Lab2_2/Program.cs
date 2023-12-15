using Microsoft.Extensions.Configuration;
using System.Net;

namespace Lab2_2
{
    internal class Program
    {
        internal static IConfiguration progConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsett.json").Build();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (Context mainContext = new Context())
            {
                mainContext.Database.EnsureDeleted();
                mainContext.Database.EnsureCreated();
                Console.WriteLine("Добавляю курсы...");
                Course[] courses = new Course[7];
                courses[0] = new Course() { Title = "MS SQL", Description = "Курс по Microsoft SQL", Duration = 5};
                courses[1] = new Course() { Title = "Git", Description = "Курс по системе управления версиями", Duration = 2 };
                courses[2] = new Course() { Title = "T-SQL", Description = "Курс по Microsoft Transact-SQL", Duration = 5 };
                courses[3] = new Course() { Title = "ibaPDA", Description = "Курс по ibaPDA", Duration = 1 };
                courses[4] = new Course() { Title = "Step-Prof", Description = "Курс по программированию на Siemens Step 7", Duration = 5 };
                courses[5] = new Course() { Title = "Tia-Prof", Description = "Курс по программированию на Tia Portal 16, степень 3", Duration = 5 };
                courses[6] = new Course() { Title = "P-M440", Description = "Курс по приводам Siemens Micromaster 440", Duration = 3 };

                foreach (Course course in courses) mainContext.Courses.Add(course);

                mainContext.SaveChanges();
                Console.WriteLine("Курсы успешно добавлены!");

                var allCourse = mainContext.Courses.OrderBy(c => c.Id).ToList();

                Console.WriteLine("Вывожу список всех курсов...");
                foreach (Course c in allCourse) Console.WriteLine($"ID: {c.Id} - Title: {c.Title} - Description: {c.Description} - Duration: {c.Duration} ");

                Course Drive = mainContext.Courses.Where(b => b.Title == "P-M440").First();
                Drive.Title = "P-G120"; Drive.Description = "Курс по приводам Siemens G120";
                mainContext.SaveChanges();
                Console.WriteLine($"Обновлен курс: ID: {Drive.Id}, {Drive.Title}, {Drive.Description}, {Drive.Duration}");



                Console.WriteLine("Создаю преподавателей...");

                Teacher[] teachers = new Teacher[5];
                teachers[0] = new Teacher() { Name = "Кулаков Геннадий Константинович", Courses = { courses[4], courses[5] } };
                teachers[1] = new Teacher() { Name = "Кобелев Александр Николаевич", Courses = { courses[1], courses[2], courses[0] } };
                teachers[2] = new Teacher() { Name = "Володин Кирилл Александрович", Courses = { courses[3], courses[2], courses[4] } };
                teachers[3] = new Teacher() { Name = "Шмаков Николай Дуринбекович", Courses = { courses[0], courses[5] } };
                teachers[4] = new Teacher() { Name = "Баринов Дмитрий Викторович", Courses = { courses[1], courses[6] } };
                foreach (Teacher teacher in teachers) mainContext.Teachers.Add(teacher);

                mainContext.SaveChanges();
                Console.WriteLine("Преподаватели успешно добавлены!");

                Console.WriteLine("Вывожу список преподавателей...");
                foreach (Teacher teacher in teachers) Console.WriteLine($"ID: {teacher.Id}, Name: {teacher.Name}");

                Console.WriteLine("Создаю студентов...");

                Student[] students = new Student[10];
                students[0] = new Student() { Name = "Бушуев Владислав Александрович", Courses = { courses[0], courses[5], courses[4] } };
                students[1] = new Student() { Name = "Пупков Сергей Николаевич", Courses = { courses[2], courses[5], courses[1] } };
                students[2] = new Student() { Name = "Кириенко Василий Леонидович", Courses = { courses[0], courses[3], courses[1] } };
                students[3] = new Student() { Name = "Сухаркин Дмитрий Ларионович", Courses = { courses[1], courses[4] } };
                students[4] = new Student() { Name = "Молотков Денис Юрьевич", Courses = { courses[3], courses[6] } };
                students[5] = new Student() { Name = "Вавилов Майкл Тамирович", Courses = { courses[2] } };
                students[6] = new Student() { Name = "Кукозябин Гордей Антонович", Courses = { courses[0], courses[3] } };
                students[7] = new Student() { Name = "Панин Сергей Петрович", Courses = { courses[1], courses[6], courses[2] } };
                students[8] = new Student() { Name = "Шапкин Джордж Тагирович", Courses = { courses[3], courses[4], courses[6] } };
                students[9] = new Student() { Name = "Папин Мерс Донднорович", Courses = { courses[1], courses[4], courses[5] } };

                foreach (Student student in students) mainContext.Students.Add(student);

                mainContext.SaveChanges();
                Console.WriteLine("Студенты успешно добавлены!");

                Console.WriteLine("Вывожу список студентов...");
                foreach (Student student in students) Console.WriteLine($"ID: {student.Id}, Name: {student.Name}");
            }
            
            using (Context listContext = new Context())
            {
                Console.WriteLine("Загружаю студентов и их курсы...");
                var students = listContext.Students.ToArray();
                var courses = listContext.Courses.ToList();
                var mass = listContext.Teachers.Select(t => t.Courses);
                foreach (Student student in students) {

                    Console.WriteLine($"Студент: {student.Name}, курс:");
                }

                foreach (Course course in courses)
                    foreach(Student stud in course.Students)
                        Console.WriteLine($"Курс: {course.Title}, студент: {stud.Name}");
            }
        }
    }
}
