namespace Lab1_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Thread.CurrentThread.Name = "Main";
            Thread t1 = new Thread(PrintNumbers) { Name = "A" }; 
            Thread t2 = new Thread(PrintNumbers) { Name = "B" };

            t1.Start();
            t2.Start(t1);

            /* Эксперимент
            t2.Start(t1);
            Thread.Sleep(1000);
            t1.Start();*/

        }

        /// <summary>
        /// Выводит числа от 1 до 100
        /// </summary>
        /// <param name="thread">Поток для ожидания</param>
        static void PrintNumbers(object? thread)
        {
            if (thread != null // проверяем что поток существует
                && thread is Thread t // типизируем объект
                && t.ThreadState != ThreadState.Unstarted /* Проверяем что поток получил команду старта, чтобы не словить исключение */) { 
                t.Join();
            }
            for (int i = 0; i <= 100; i++)
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.Name}, value: {i}");
            }
        }
    }
}
