namespace Non_coop.Multithread
{
    public class SupermarketInventory
    {

        public SupermarketInventory(int numThreads, int threadRuns)
        {
            NumThreads = numThreads;
            ThreadRuns = threadRuns;
        }

        private int Money { get; set; } = 0;
        private int ThreadRuns { get; set; }
        private int NumThreads { get; set; }
        private List<Bill> Bills { get; set; } = new List<Bill>();
        private static int readerTimeOut = 100000;
        private static int writerTimeOut = 100000;
        private static ReaderWriterLockSlim InventoryLock { get; set; } = new();
        private static ReaderWriterLockSlim BillsLock { get; set; } = new();
        private static Product Milk { get; set; } = new Product { Type = "Milk", UnitPrice = 5, Quantity = 1054509 };
        private static Product Bread { get; set; } = new Product { Type = "Bread", UnitPrice = 6, Quantity = 1219789 };
        private static Product Water { get; set; } = new Product { Type = "Water", UnitPrice = 3, Quantity = 2006689 };
        private static Product Pasta { get; set; } = new Product { Type = "Pasta", UnitPrice = 4, Quantity = 2387819 };
        private static Product Mug { get; set; } = new Product { Type = "Mug", UnitPrice = 20, Quantity = 4306459 };
        private bool IsPrime(int a)
        {
            if (a < 3 && a >= 0) return true;
            int i;
            for (i = 2; i * i < a; i++)
            {
                if (a % i == 0) return false;
            }
            if (a % i == 0) return false;
            return true;
        }

        public void Sales()
        {
            Thread[] threads = new Thread[NumThreads];
            for (int i = 0; i < NumThreads; i++)
            {
                threads[i] = new(new ThreadStart(Sale))
                {
                    Name = string.Format("{0}", i + 1)
                };
                threads[i].Start();
            }
            for (int i = 0; i < NumThreads; i++)
                threads[i].Join();
            try
            {

                InventoryLock.TryEnterReadLock(readerTimeOut);
                // Console.WriteLine("-------FI`NAL INVENTORY--------");
                Inventory();
                InventoryLock.ExitReadLock();
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref readerTimeOut);
            }

        }

        public void Sale()
        {
            for (int i = 0; i < ThreadRuns; i++)
            {
                // If the number i is prime we will add only milk, bread and pasta, to a bill, otherwise all the rest
                // Also if the number i can be divided by 3 and inventory will be done
                if (i % 2 == 0 || i % 5 == 0 || Thread.CurrentThread.Name!.Contains("13"))
                {
                    SaleType1(i);
                }
                else
                {
                    SaleType2(i);
                }
                if (IsPrime(i))
                {
                    try
                    {
                        InventoryLock.TryEnterReadLock(readerTimeOut);
                        Inventory();
                        InventoryLock.ExitReadLock();
                    }
                    catch (ApplicationException)
                    {
                        Interlocked.Increment(ref readerTimeOut);
                    }
                }
            }
        }

        public void SaleType1(int i)
        {
            try
            {
                // Bill and product handling
                // BillsLock.TryEnterWriteLock(writerTimeOut);
                Bill Sale1Bill = new();
                // BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Sale1Bill.QuantitiesSold.Add(2 + i);
                Sale1Bill.ProductsSold.Add(Milk.Copy(Milk));
                Sale1Bill.BillPrice += (2 + i) * Milk.UnitPrice;
                Milk.Quantity -= 2 + i;
                System.Console.WriteLine("Start: {0}", Thread.CurrentThread.Name);
                Thread.Sleep(5000);
                System.Console.WriteLine("Stop: {0}", Thread.CurrentThread.Name);
                BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Sale1Bill.QuantitiesSold.Add(4 + i);
                Sale1Bill.ProductsSold.Add(Bread.Copy(Bread));
                Sale1Bill.BillPrice += (4 + i) * Bread.UnitPrice;
                Bread.Quantity -= 4 + i;
                BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Sale1Bill.QuantitiesSold.Add(5 + i);
                Sale1Bill.ProductsSold.Add(Pasta.Copy(Pasta));
                Sale1Bill.BillPrice += (5 + i) * Pasta.UnitPrice;
                Pasta.Quantity -= 5 + i;
                BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Money += Sale1Bill.BillPrice;
                Bills.Add(Sale1Bill);
                BillsLock.ExitWriteLock();
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
        }

        public void SaleType2(int i)
        {
            try
            {
                // Bill and product handling
                BillsLock.TryEnterWriteLock(writerTimeOut);
                Bill Sale2Bill = new();
                BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Sale2Bill.QuantitiesSold.Add(5 + i);
                Sale2Bill.ProductsSold.Add(Water.Copy(Water));
                Sale2Bill.BillPrice += (5 + i) * Water.UnitPrice;
                Water.Quantity -= 5 + i;
                BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Sale2Bill.QuantitiesSold.Add(3 + i);
                Sale2Bill.ProductsSold.Add(Mug.Copy(Mug));
                Sale2Bill.BillPrice += (3 + i) * Mug.UnitPrice;
                Mug.Quantity -= 3 + i;
                BillsLock.ExitWriteLock();

                BillsLock.TryEnterWriteLock(writerTimeOut);
                Money += Sale2Bill.BillPrice;
                Bills.Add(Sale2Bill);
                BillsLock.ExitWriteLock();

            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
        }

        public bool Inventory()
        {
            int TotalSales = 0;

            // Console.WriteLine("\t\t\tINVENTORY\n");
            for (int i = 0; i < Bills.Count; i++)
            {
                TotalSales += Bills[i].BillPrice;
            }
            // Console.WriteLine(string.Format("\n\nTotal Sales: {0}, Money made today: {0}", TotalSales, Money));
            if (Money != TotalSales) return false;
            return true;
        }

    }
}