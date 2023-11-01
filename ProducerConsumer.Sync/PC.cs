namespace ProducerConsumer.Sync
{
    public static class PC
    {
        private static int[] vectorA = { 1, 2, 3, 4, 5 }; // Sample vector A
        private static int[] vectorB = { 5, 4, 3, 2, 1 }; // Sample vector B
        private static int[] products = new int[vectorA.Length]; // Array to store products
        private static bool produced = false; // Flag to indicate if products have been produced
        private static int sum = 0; // Sum of products
        private static Mutex mutex = new(); // Mutex to control access to products array

        public static void Run()
        {
            Thread producer = new(Producer);
            Thread consumer = new(Consumer);

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();
            Console.WriteLine($"Sum of products is {sum}");
        }

        private static void Producer()
        {
            for (int i = 0; i < vectorA.Length; i++)
            {
                mutex.WaitOne(); // Wait for mutex to be available
                products[i] = vectorA[i] * vectorB[i];
                produced = true;
                mutex.ReleaseMutex(); // Release mutex
            }
        }

        private static void Consumer()
        {
            for (int i = 0; i < vectorA.Length; i++)
            {
                while (!produced)
                    mutex.WaitOne(); // Wait for mutex to be available
                Console.WriteLine($"Product of {vectorA[i]} and {vectorB[i]} is {products[i]}");
                sum += products[i];
                produced = false;
                mutex.ReleaseMutex(); // Release mutex
            }
        }

    }
}