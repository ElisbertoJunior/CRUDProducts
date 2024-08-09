namespace ProductAPI.Models
{
    public class Department
    {
        public string Code { get; set; }

        public string Description { get; set; }

        // Relacionamento de um para muitos com Product
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
