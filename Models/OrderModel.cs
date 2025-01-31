namespace ShoesShop.Models
{
    public class OrderModel
    {
        public int? OrderId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string MobileNumber { get; set; }

    }
}
