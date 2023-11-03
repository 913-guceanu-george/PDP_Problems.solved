namespace Non_coop.Multithread
{
    public class Product
    {
        public string Type { get; set; } = string.Empty;
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Product Copy(Product toCopy)
        {
            return new Product { Type = toCopy.Type, UnitPrice = toCopy.UnitPrice, Quantity = toCopy.Quantity };
        }
        public override string ToString()
        {
            return string.Format("Type: {0}, Unit price: {1}, Quantity: {2}", Type, UnitPrice, Quantity);
        }
    }
}