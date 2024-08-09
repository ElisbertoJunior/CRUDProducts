namespace ProductAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }

        // Chave estrangeira para Departemnt
        public int DepartmentId { get; set; }
        public Department Department { get; set; }


    }
}
