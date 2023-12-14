using System.Diagnostics;

namespace BinaryBalancedTreeMT
{
    public class TreeNode
    {

        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public int Weight { get; set; }

    }


    internal class Program
    {
        static Random rnd = new Random();
        static long total;

        public static void CreateRandomTree(TreeNode node, int level)
        {
            node.Left = new TreeNode();
            node.Right = new TreeNode();
            node.Weight = rnd.Next(100);
            total += node.Weight;
            level--;
            if (level == 0)
            {
                node.Left.Weight = rnd.Next(100);
                node.Right.Weight = rnd.Next(100);
                total += node.Left.Weight;
                total += node.Right.Weight;
                return;
            }
            CreateRandomTree(node.Left, level);
            CreateRandomTree(node.Right, level);
        }


        public static long weightTree(TreeNode root)
        {
            return
                (long)root.Weight +
                (root.Left != null ? weightTree(root.Left) : 0) +
                (root.Right != null ? weightTree(root.Right) : 0);

        }
        public static async Task<long> weightTreeMulti(TreeNode root, int level = 4)
        {
            if (level <= 0) return weightTree(root);
            int newLevel = level - 1;

            // асинхронные задачи запускаем параллельно (bugfix)
            Task<long> t1 = null, t2 = null;
            if (root.Left != null)
                t1 = Task.Run(() => weightTreeMulti(root.Left, newLevel));
            if (root.Right != null)
                t2 = Task.Run(() => weightTreeMulti(root.Right, newLevel));

            return
                (long)root.Weight +
                (t1 != null ? await t1 : 0) +
                (t2 != null ? await t2 : 0);
        }

        static void Main(string[] args)
        {
            int treeLevel = 26; // 2^(n+1)-1

            Console.WriteLine($"Starting tree creation with depth {treeLevel}...");
            TreeNode root = new TreeNode();
            CreateRandomTree(root, treeLevel);
            Console.WriteLine($"Tree created with total weight: {total}");

            ThreadPool.SetMinThreads(32, 32);
            ThreadPool.SetMaxThreads(64, 64);
            // optimization run
            Task.WaitAll(
                weightTreeMulti(root), weightTreeMulti(root), weightTreeMulti(root)
            );

            // test run
            Stopwatch t2 = new Stopwatch();
            t2.Start();
            long r2 = weightTreeMulti(root).Result;
            t2.Stop();
            Console.WriteLine($"Multi  weight: {r2} Time {t2.ElapsedMilliseconds}");

            // optimization run
            weightTree(root); weightTree(root); weightTree(root);

            // test run
            Stopwatch t1 = new Stopwatch();
            t1.Start();
            long r1 = weightTree(root);
            t1.Stop();
            Console.WriteLine($"Single weight: {r1} Time {t1.ElapsedMilliseconds}");

        }
    }
}