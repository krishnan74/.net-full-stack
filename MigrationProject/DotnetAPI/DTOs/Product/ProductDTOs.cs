namespace DotnetAPI.DTOs.Product
{
    public class AddProductDTO
    {
        public string ProductName { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? ColorId { get; set; }
        public int? ModelId { get; set; }
        public int? StorageId { get; set; }
        public System.DateTime? SellStartDate { get; set; }
        public System.DateTime? SellEndDate { get; set; }
        public int? IsNew { get; set; }
    }

    public class UpdateProductDTO
    {
        public string ProductName { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? ColorId { get; set; }
        public int? ModelId { get; set; }
        public int? StorageId { get; set; }
        public System.DateTime? SellStartDate { get; set; }
        public System.DateTime? SellEndDate { get; set; }
        public int? IsNew { get; set; }
    }
}
