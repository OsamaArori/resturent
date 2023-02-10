namespace WebApplication1.Models
{
    public class JoinTable
    {
        public Category category { get; set; }
        public Customer customer { get; set; }
        public Product product { get; set; }
        public ProductCustomer productCustomer { get; set; }
    }
}
