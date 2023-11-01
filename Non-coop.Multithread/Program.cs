using Non_coop.Multithread;

internal class Program
{
    private static void Test100Threads()
    {
        DateTime start = DateTime.Now;
        new SupermarketInventory(128, 5120).Sales();
        DateTime end = DateTime.Now;
        Console.WriteLine("Time elapsed for 128 threads: {0}", end - start);
    }

    private static void Test200Threads()
    {
        DateTime start = DateTime.Now;
        new SupermarketInventory(256, 2560).Sales();
        DateTime end = DateTime.Now;
        Console.WriteLine("Time elapsed for 256 threads: {0}", end - start);
    }

    private static void Test400Threads()
    {
        DateTime start = DateTime.Now;
        new SupermarketInventory(512, 1280).Sales();
        DateTime end = DateTime.Now;
        Console.WriteLine("Time elapsed for 512 threads: {0}", end - start);
    }

    private static void Main(string[] args)
    {
        Test100Threads();
        Test200Threads();
        Test400Threads();
    }
}