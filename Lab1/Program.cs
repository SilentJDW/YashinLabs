namespace Lab1
{
    public class Params
    {
        public int startPos { get; init; }
        public int endPos { get; init; }
        public void Start ()
        {
            for (int i = startPos; i <= endPos; i++)
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.Name}, value: {i}");
            }
        }
    }

    record class SugarParams (int start, int end);
    internal class Program
    {
        private static void PrintParams (object? p)
        {
            if (p is SugarParams @params)
            {
                for (int i = @params.start; i <= @params.end; i++)
                {
                    Console.WriteLine($"Thread: {Thread.CurrentThread.Name}, value: {i}");
                }
            }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(PrintParams) { Name = "A" };
            Thread t2 = new Thread(new Params() { startPos = 1, endPos = 4}.Start) { Name = "B"};

            t1.Start(new SugarParams(4, 6));
            t2.Start();
        }
    }
}