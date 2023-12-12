namespace Lab1_3
{
    record class SyncObj(double n);

    public class MyObject
    {
        public double n;
        public bool waitCos = false;
        public void Cos()
        {
            for (int i = 0; i < 10; i++)
            {
                lock (this)
                {
                    while (waitCos) { Monitor.Wait(this); }
                    n = Math.Cos(n);
                    Console.WriteLine($"Thread: {Thread.CurrentThread.Name}, value: {n}");
                    waitCos = true;
                    Monitor.Pulse(this);

                }
            }
        }

        public void ArkCos()
        {
            for (int a = 0; a < 10; a++)
            {
                lock (this)
                {
                    while (!waitCos) { Monitor.Wait(this); }
                    n = Math.Acos(n);
                    Console.WriteLine($"Thread: {Thread.CurrentThread.Name}, value: {n}");
                    waitCos = false;
                    Monitor.Pulse(this);
                }
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            MyObject sync = new MyObject() { n = 1.0 };
            Thread t1 = new Thread(sync.Cos) { Name = "A" };
            Thread t2 = new Thread(sync.ArkCos) { Name = "B" };

            t1.Start();
            t2.Start();
        }
    }
}
