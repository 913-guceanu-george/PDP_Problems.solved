namespace Non_coop.Multithread
{
    public class SupermarketInventory
    {

        public SupermarketInventory(int numThreads)
        {
            NumThreads = numThreads;
        }

        private static Bill currentBill { get; set; } = new();
        private int Money { get; set; } = 0;
        private int ThreadRuns { get; set; } = 5;
        private int NumThreads { get; set; }
        private static int readerTimeOut = 100;
        private static int writerTimeOut = 100;
        private static ReaderWriterLock InventoryLock { get; set; } = new();
        private List<Bill> Bills { get; set; } = new List<Bill>();
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
            Console.WriteLine("-------FINAL INVENTORY--------");
            try
            {
                try
                {
                    InventoryLock.AcquireReaderLock(readerTimeOut);
                    Inventory();
                }
                finally
                {
                    InventoryLock.ReleaseReaderLock();
                }
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
                currentBill = new();
                if (IsPrime(i))
                {
                    SaleType1(i);
                    try
                    {
                        try
                        {
                            currentBill.Lock.AcquireWriterLock(writerTimeOut);
                            Bills.Add(currentBill);
                        }
                        finally
                        {
                            currentBill.Lock.ReleaseWriterLock();
                        }
                    }
                    catch (ApplicationException)
                    {
                        Interlocked.Increment(ref writerTimeOut);
                    }
                }
                else
                {
                    SaleType2(i);
                    try
                    {
                        try
                        {
                            currentBill.Lock.AcquireWriterLock(writerTimeOut);
                            Bills.Add(currentBill);
                        }
                        finally
                        {
                            currentBill.Lock.ReleaseWriterLock();
                        }
                    }
                    catch (ApplicationException)
                    {
                        Interlocked.Increment(ref writerTimeOut);
                    }
                }
                Money += currentBill.BillPrice;
                if (i % 3 == 0)
                {
                    try
                    {
                        try
                        {
                            InventoryLock.AcquireReaderLock(readerTimeOut);
                            Inventory();
                        }
                        finally
                        {
                            InventoryLock.ReleaseReaderLock();
                        }
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

            // Write locking milk
            try
            {
                try
                {
                    Milk.Lock.AcquireWriterLock(writerTimeOut);
                    currentBill.QuantitiesSold.Add(2 + i);
                    Milk.Quantity -= 2 + i;
                    currentBill.BillPrice += (2 + i) * Milk.UnitPrice;
                    currentBill.ProductsSold.Add(Milk);
                }
                finally
                {
                    Milk.Lock.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
            // Write locking bread
            try
            {
                try
                {
                    Bread.Lock.AcquireWriterLock(writerTimeOut);
                    currentBill.QuantitiesSold.Add(4 + i);
                    Bread.Quantity -= 4 + i;
                    currentBill.BillPrice += (4 + i) * Bread.UnitPrice;
                    currentBill.ProductsSold.Add(Bread);
                }
                finally
                {
                    Bread.Lock.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
            // Write locking pasta
            try
            {
                try
                {
                    Pasta.Lock.AcquireWriterLock(writerTimeOut);
                    currentBill.QuantitiesSold.Add(5 + i);
                    Pasta.Quantity -= 5 + i;
                    currentBill.BillPrice += (5 + i) * Pasta.UnitPrice;
                    currentBill.ProductsSold.Add(Pasta);
                }
                finally
                {
                    Pasta.Lock.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
            // Read locking the current bill
            try
            {
                try
                {
                    currentBill.Lock.AcquireReaderLock(readerTimeOut);
                    Console.WriteLine("Thread {0} is executing the following bill: \n{1}", Thread.CurrentThread.Name, currentBill.ToString());
                }
                finally
                {
                    currentBill.Lock.ReleaseLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref readerTimeOut);
            }
        }

        public void SaleType2(int i)
        {

            // Write locking water
            try
            {
                try
                {
                    Water.Lock.AcquireWriterLock(writerTimeOut);
                    currentBill.QuantitiesSold.Add(5 + i);
                    Water.Quantity -= 5 + i;
                    currentBill.BillPrice += (5 + i) * Water.UnitPrice;
                    currentBill.ProductsSold.Add(Water);
                }
                finally
                {
                    Water.Lock.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
            // Write locking mug
            try
            {
                try
                {
                    Mug.Lock.AcquireWriterLock(writerTimeOut);
                    currentBill.QuantitiesSold.Add(3 + i);
                    Mug.Quantity -= 3 + i;
                    currentBill.BillPrice += (3 + i) * Mug.UnitPrice;
                    currentBill.ProductsSold.Add(Mug);
                }
                finally
                {
                    Mug.Lock.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref writerTimeOut);
            }
            // Read locking the current bill
            try
            {
                try
                {
                    currentBill.Lock.AcquireReaderLock(readerTimeOut);
                    Console.WriteLine("Thread {0} is executing the following bill: \n{1}", Thread.CurrentThread.Name, currentBill.ToString());
                }
                finally
                {
                    currentBill.Lock.ReleaseLock();
                }
            }
            catch (ApplicationException)
            {
                Interlocked.Increment(ref readerTimeOut);
            }

        }

        public bool Inventory()
        {
            int TotalSales = 0;

            Console.WriteLine("\t\t\tINVENTORY\n");
            Console.WriteLine("==================================");
            for (int i = 0; i < Bills.Count; i++)
            {
                Console.WriteLine(string.Format("Bill no.{0} ", i + 1) + "\n" + Bills[i].ToString() + "\n");
                TotalSales += Bills[i].BillPrice;
                Console.WriteLine(string.Format("TOTAL SALES SO FAR: {0}", TotalSales));
            }
            Console.WriteLine(string.Format("\n\nTotal Sales: {0}, Money made today: {0}", TotalSales, Money));

            Console.WriteLine("==================================");
            if (Money != TotalSales) return false;
            return true;
        }

    }
}