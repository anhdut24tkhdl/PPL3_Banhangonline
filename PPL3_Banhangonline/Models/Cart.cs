namespace PPL3_Banhangonline.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int CustomerID { get; set; }

        public Customer? Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
