namespace Non_coop.Multithread
{
    public class Product
    {
        public string Type { get; set; } = string.Empty;
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public ReaderWriterLock Lock { get; set; } = new();
        public override string ToString()
        {
            return "Type: " + Type.ToString() + " Unit price: " + UnitPrice.ToString() + " Quantity : " + Quantity.ToString();
        }
    }

}