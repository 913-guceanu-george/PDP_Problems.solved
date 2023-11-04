using Non_coop.Multithread;

internal class Program
{
    private static string total = string.Empty;
    private static void TestNThreads(int threadCount, int threadRuns)
    {
        DateTime start = DateTime.Now;
        new SupermarketInventory(threadCount, threadRuns).Sales();
        DateTime end = DateTime.Now;
        total += string.Format("Time elapsed for {0} threads: {1}\n", threadCount, end - start);
    }
    private static void Main(string[] args)
    {
        TestNThreads(64, 102400);
        // TestNThreads(128, 51200);
        // TestNThreads(256, 25600);
        // TestNThreads(512, 12800);
        // TestNThreads(2, 10);
        Console.WriteLine(total);
    }
}