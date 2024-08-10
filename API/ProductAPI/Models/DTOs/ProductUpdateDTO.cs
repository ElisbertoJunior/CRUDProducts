namespace ProductAPI.Models.DTOs
{
    public class ProductUpdateDTO
    {
        
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public int DepartmentId { get; set; }
    }
}
