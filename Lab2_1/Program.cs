using Microsoft.Extensions.Configuration;

namespace Lab2_1
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
            }
            
        }
    }
}
