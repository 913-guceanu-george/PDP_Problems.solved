namespace Non_coop.Multithread
{
    public class SupermarketInventory
    {

        public SupermarketInventory()
        {
            this.Milk = new Product { Type = "Milk", UnitPrice = 5, Quantity = 1009 };
            this.Bread = new Product { Type = "Bread", UnitPrice = 6, Quantity = 1219 };
            this.Water = new Product { Type = "Water", UnitPrice = 3, Quantity = 2009 };
            this.Pasta = new Product { Type = "Pasta", UnitPrice = 4, Quantity = 2319 };
            this.Mug = new Product { Type = "Mug", UnitPrice = 20, Quantity = 4309 };
        }
        int Money { get; set; } = 0;
        int ThreadRuns { get; set; } = 12;
        int NumThreads = 3;
        List<Bill> Bills { get; set; } = new List<Bill>();
        Product Milk { get; set; }
        private static Mutex MilkMutex { get; set; } = new Mutex();
        Product Bread { get; set; }
        private static Mutex BreadMutex { get; set; } = new Mutex();
        Product Water { get; set; }
        private static Mutex WaterMutex { get; set; } = new Mutex();
        Product Pasta { get; set; }
        private static Mutex PastaMutex { get; set; } = new Mutex();
        Product Mug { get; set; }
        private static Mutex MugMutex { get; set; } = new Mutex();
        private bool IsPrime(int a)
        {
            if (a < 3 && a > 0) return true;
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
            for (int i = 0; i < NumThreads; i++)
            {
                Thread NewThread = new(new ThreadStart(Sale))
                {
                    Name = string.Format("{0}", i + 1)
                };
                // if (!Inventory()) { Console.WriteLine("STEALING!!!"); }
                NewThread.Start();
            }
            Console.WriteLine("-------FINAL INVENTORY--------");
            Inventory();
        }

        public void Sale()
        {
            // TODO - create thread activity, randomize the way the sale is to be done
            for (int i = 0; i < ThreadRuns; i++)
            {
                // If the number i is prime we will add only milk, bread and pasta, to a bill, otherwise all the rest
                // Also if the number i can be divided by 3 and inventory will be done
                Bill currentBill = new();
                if (IsPrime(i))
                {
                    SaleType1(currentBill, i);
                    Bills.Add(currentBill);
                }
                else
                {
                    SaleType2(currentBill, i);
                    Bills.Add(currentBill);
                }
                Money += currentBill.BillPrice;
                if (i % 3 == 0)
                {
                }
                // Console.WriteLine(string.Format("{0} is doing the following bill: \n", Thread.CurrentThread.Name));
                Console.WriteLine("Thread: " + Thread.CurrentThread.Name + "\n" + currentBill.ToString());
                // Thread.Sleep(10000);
            }
        }

        public void SaleType1(Bill currentBill, int i)
        {
            MilkMutex.WaitOne();
            try
            {
                currentBill.ProductsSold.Add(Milk.Copy());
                currentBill.QuantitiesSold.Add(2 + i);
                Milk.Quantity -= 2 + i;
                currentBill.BillPrice += (2 + i) * Milk.UnitPrice;
            }
            finally
            {
                MilkMutex.ReleaseMutex();
            }

            BreadMutex.WaitOne();
            try
            {
                currentBill.ProductsSold.Add(Bread.Copy());
                currentBill.QuantitiesSold.Add(4 * i);
                Bread.Quantity -= 4 * i;
                currentBill.BillPrice += 4 * i * Bread.UnitPrice;
            }
            finally
            {
                BreadMutex.ReleaseMutex();
            }

            PastaMutex.WaitOne();
            try
            {
                currentBill.ProductsSold.Add(Pasta.Copy());
                currentBill.QuantitiesSold.Add(3 * i - 1);
                Pasta.Quantity -= 3 * i - 1;
                currentBill.BillPrice += (3 * i - 1) * Milk.UnitPrice;
            }
            finally
            {
                PastaMutex.ReleaseMutex();
            }
        }

        public void SaleType2(Bill currentBill, int i)
        {
            WaterMutex.WaitOne();
            try
            {
                currentBill.ProductsSold.Add(Water.Copy());
                currentBill.QuantitiesSold.Add(5 + i);
                Water.Quantity -= 5 + i;
                currentBill.BillPrice += (5 + i) * Milk.UnitPrice;
            }
            finally
            {
                WaterMutex.ReleaseMutex();
            }

            MugMutex.WaitOne();
            try
            {
                currentBill.ProductsSold.Add(Mug.Copy());
                currentBill.QuantitiesSold.Add(3 * i);
                Mug.Quantity -= 3 * i;
                currentBill.BillPrice += 3 * i * Milk.UnitPrice;
            }
            finally
            {
                MugMutex.ReleaseMutex();
            }
        }

        public bool Inventory()
        {
            int TotalSales = 0;
            Console.WriteLine("\t\t\tINVENTORY\n");
            Console.WriteLine("==================================");
            for (int i = 0; i < Bills.Count; i++)
            {
                Console.WriteLine(string.Format("Bill no.{0} ", i + 1) + "on thread " + Thread.CurrentThread.Name + Bills[i].ToString() + "\n");
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