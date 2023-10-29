namespace Non_coop.Multithread
{
    public class Bill
    {
        public List<Product> ProductsSold { get; set; } = new();
        public List<int> QuantitiesSold { get; set; } = new();
        public int BillPrice { get; set; } = 0;
        public string ThreadName { get; set; } = string.Empty;
        public ReaderWriterLock Lock { get; set; } = new();
        public override string ToString()
        {
            string fin = "";
            for (int i = 0; i < ProductsSold.Count; i++)
            {
                fin += string.Format("{0}, ==> Quantities sold: ", ProductsSold[i].ToString()) + QuantitiesSold[i].ToString() + "\n";
            }
            fin += string.Format("Final bill price : {0}", BillPrice) + "\n" + " on thread" + ThreadName + "\n";
            return fin;
        }
    }
}