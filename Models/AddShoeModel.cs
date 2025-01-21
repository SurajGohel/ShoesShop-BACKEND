namespace ShoesShop.Models
{
    public class AddShoeModel
    {
        public int? ShoeId { get; set; } // Corresponds to ShoeId (Primary Key)
        public string Name { get; set; } // Corresponds to Name
        public int CategoryId { get; set; } // Corresponds to CategoryId (Foreign Key referencing Categories table)
        
        public double Price { get; set; } // Corresponds to Price
        public IFormFile Image { get; set; } // Corresponds to Image (stored as binary data)
        public string Description { get; set; } // Corresponds to Description
        public int Stock { get; set; } // Corresponds to Stock
        public string? ImageURL { get; set; }
    }
}
