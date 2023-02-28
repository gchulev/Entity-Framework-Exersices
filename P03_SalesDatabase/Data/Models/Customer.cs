namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }
        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CreditCardNumber { get; set; } = null!;
        public ICollection<Sale>? Sales { get; set; }
    }
}
