namespace Non_coop.Multithread
{
    public class Product
    {
        public string Type { get; set; } = string.Empty;
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public override string ToString()
        {
            return "Type: " + Type.ToString() + " Unit price: " + UnitPrice.ToString() + " Quantity : " + Quantity.ToString();
        }
        public Product Copy()
        {
            return new Product { Type = this.Type, UnitPrice = this.UnitPrice, Quantity = this.Quantity };
        }
    }

}