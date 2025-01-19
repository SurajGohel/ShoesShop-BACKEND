namespace ShoesShop.Models
{
    public class UserModel
    {
        public int? UserId { get; set; } // Corresponds to UserId (Primary Key)
        public string Name { get; set; } // Corresponds to Name
        public string Email { get; set; } // Corresponds to Email
        public string Password { get; set; } // Corresponds to Password
        public string Role { get; set; } // Corresponds to Role
        public DateTime CreatedDate { get; set; } // Corresponds to CreatedDate
        public DateTime ModifiedDate { get; set; } // Corresponds to ModifiedDate
    }

    public class UserLoginModel
    {
        public string Email { get; set; } // Corresponds to Email
        public string Password { get; set; } // Corresponds to Password
        public string Role { get; set; } // Corresponds to Role
    }
}
