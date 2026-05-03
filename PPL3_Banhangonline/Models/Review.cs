namespace PPL3_Banhangonline.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        public int ProductID { get; set; }
        public Product? Product { get; set; }

        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        public int OrderID { get; set; }
        public Order? Order { get; set; }

        public int Rating { get; set; } // 1 - 5
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}