namespace Non_coop.Multithread
{
    public class Bill
    {
        public List<Product> ProductsSold { get; set; } = new();
        public List<int> QuantitiesSold { get; set; } = new();
        public int BillPrice { get; set; } = 0;
        public ReaderWriterLockSlim Lock { get; set; } = new();
        public override string ToString()
        {
            string fin = "";
            for (int i = 0; i < ProductsSold.Count; i++)
            {
                fin += string.Format("{0}, Sold => {1} units\n", ProductsSold[i].ToString(), QuantitiesSold[i]);
            }
            fin += string.Format("Final bill price : {0}", BillPrice) + "\n";
            return fin;
        }
    }
}