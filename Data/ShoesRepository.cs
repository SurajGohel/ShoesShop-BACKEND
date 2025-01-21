using Microsoft.Data.SqlClient;
using ShoesShop.Models;
using ShoesShop.Utils;
using System.Data;

namespace ShoesShop.Data
{
    public class ShoesRepository
    {
        private readonly string _connectionString;
        public readonly IConfiguration _configuration;
        public ShoesRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public IEnumerable<ShoesModel> SelectAll()
        {
            var shoes = new List<ShoesModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    shoes.Add(new ShoesModel()
                    {
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Name = reader["Name"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),
                        ImageURL = reader["ImageURL"].ToString(),
                        Description = reader["Description"].ToString(),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                    });
                }
            }
            return shoes;
        }

       

        public ShoeDetailModel SelectByID(int shoeId)
        {
            ShoeDetailModel shoe = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_ShoeDetail", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    shoe = new ShoeDetailModel
                    {
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Name = reader["Name"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),
                        //ImageURL = reader["ImageURL"].ToString(),
                        //Image = reader["Image"] as byte[],
                        Description = reader["Description"].ToString(),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        AverageRating = reader["AverageRating"] != DBNull.Value ? Convert.ToDouble(reader["AverageRating"]) : (double?)null,
                        TotalReviews = Convert.ToInt32(reader["TotalReviews"]),
                        UserName = reader["UserName"].ToString(),
                        Comment = reader["Comment"].ToString(),
                        ReviewDate = reader["ReviewDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReviewDate"]) : (DateTime?)null,
                        Rating = reader["Rating"] != DBNull.Value ? Convert.ToInt32(reader["Rating"]) : (int?)null
                    };
                }
            }
            return shoe;
        }

        public ShoesModel SelectByPK(int shoeId)
        {
            ShoesModel shoe = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    shoe = new ShoesModel
                    {
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Name = reader["Name"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),
                        //ImageURL = reader["ImageURL"].ToString(),
                        //Image = reader["Image"] as byte[],
                        Description = reader["Description"].ToString(),
                        Stock = Convert.ToInt32(reader["Stock"])
                    };
                }
            }
            return shoe;
        }

        public IEnumerable<ShoesModel> GetShoesByCategoryId(int categoryId)
        {
            var shoes = new List<ShoesModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_SelectByCategoryId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    shoes.Add(new ShoesModel
                    {
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Name = reader["Name"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString(),

                        Price = Convert.ToDouble(reader["Price"]),
                        //ImageURL = reader["ImageURL"].ToString(),
                        //Image = reader["Image"] as byte[],
                        Description = reader["Description"].ToString(),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                    });
                }
            }
            return shoes;
        }


        public IEnumerable<ShoesModel> SearchShoesByName(string shoeName)
        {
            var shoes = new List<ShoesModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_SearchByName", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShoeName", shoeName);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    shoes.Add(new ShoesModel
                    {
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Name = reader["Name"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),
                        //ImageURL = reader["ImageURL"].ToString(),
                        //Image = reader["Image"] as byte[],
                        Description = reader["Description"].ToString(),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                    });
                }
            }
            return shoes;
        }

        public async Task<bool> Insert(AddShoeModel shoe)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", shoe.Name);
                cmd.Parameters.AddWithValue("@CategoryId", shoe.CategoryId);
                cmd.Parameters.AddWithValue("@Price", shoe.Price);


                CloudinaryService cloudinaryService = new CloudinaryService(this._configuration);
                string url = await cloudinaryService.UploadFileAsync(shoe.Image);
                cmd.Parameters.AddWithValue("@ImageURL", url);



                //cmd.Parameters.AddWithValue("@Image", shoe.Image);
                cmd.Parameters.AddWithValue("@Description", shoe.Description);
                cmd.Parameters.AddWithValue("@Stock", shoe.Stock);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Update(AddShoeModel shoe)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_Edit", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShoeId", shoe.ShoeId);
                cmd.Parameters.AddWithValue("@Name", shoe.Name);
                cmd.Parameters.AddWithValue("@CategoryId", shoe.CategoryId);
                cmd.Parameters.AddWithValue("@Price", shoe.Price);
                cmd.Parameters.AddWithValue("@ImageURL", shoe.ImageURL);
                //cmd.Parameters.AddWithValue("@Image", shoe.Image);
                cmd.Parameters.AddWithValue("@Description", shoe.Description);
                cmd.Parameters.AddWithValue("@Stock", shoe.Stock);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int shoeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Shoes_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

     
    }
}
