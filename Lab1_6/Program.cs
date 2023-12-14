using System.Diagnostics;

namespace Lab1_6
{
    internal class Program
    {
        const int STEPS = 100000000;
        const int TASKS = 10;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Stopwatch t1 = new Stopwatch();
            t1.Start();
            double r1 = Single(Math.Sin, 0, Math.PI / 2);
            t1.Stop();
            Console.WriteLine($"Single result : {r1} Time: {t1.ElapsedMilliseconds}");

            Stopwatch t2 = new Stopwatch();
            t2.Start();
            double r2 = Multi(Math.Sin, 0, Math.PI / 2);
            t2.Stop();
            Console.WriteLine($"Multi  result : {r2} Time: {t2.ElapsedMilliseconds}");
        }
        static double Single(Func<double, double> f, double a, double b, int steps = STEPS)
        {
            double w = (b - a) / steps;
            double summa = 0d;
            for (int i = 0; i < steps; i++)
            {
                double x = a + i * w + w / 2;
                double h = f(x);
                summa += h * w;
            }
            return summa;
        }

        static double Multi(Func<double, double> f, double a, double b)
        {
            double w = (b - a) / TASKS;
            double summa = 0;
            var s = new object();
            Parallel.For(0, TASKS, i =>
            {
                double r = Single(f, a + i * w, a + (i + 1) * w, STEPS / TASKS);
                lock (s) summa += r;
            });
            return summa;
        }
    }
}


