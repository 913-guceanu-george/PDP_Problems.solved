namespace ProducerConsumer.Sync
{
    public static class PC
    {
        private static int[] VectorA = { 1, 2, 3, 4, 5 }; // Sample vector A
        private static int[] vectorB = { 5, 4, 3, 2, 1 }; // Sample vector B
        private static int[] Products = new int[VectorA.Length]; // Array to store Products
        private static readonly Thread ProducerThread = new(Producer) { Name = "Producer" };
        private static readonly Thread ConsumerThread = new(Consumer) { Name = "Consumer" };
        private static object cond = new();
        private static int sum = 0; // Sum of Products
        private static bool isProduced = false;

        public static void Run()
        {
            Thread.CurrentThread.Name = "Main";

            ProducerThread.Start();
            ConsumerThread.Start();

            ProducerThread.Join();
            ConsumerThread.Join();
        }

        private static void Producer()
        {
            for (int i = 0; i < VectorA.Length; i++)
            {
                lock (cond)
                {
                    Console.WriteLine($"Producer: {i}");
                    Products[i] = VectorA[i] * vectorB[i];
                    Monitor.PulseAll(cond);
                }
            }
            isProduced = true;
        }

        private static void Consumer()
        {
            for (int i = 0; i < VectorA.Length; i++)
            {
                lock (cond)
                {
                    Monitor.PulseAll(cond);
                    Console.WriteLine($"Consumer: {i}");
                    sum += Products[i];
                    Console.WriteLine($"Product of {VectorA[i]} and {vectorB[i]} is {Products[i]}");
                    Monitor.Wait(cond);
                }
                if (isProduced) break;
            }
            Console.WriteLine($"Sum of products is {sum}");

        }
    }
}
