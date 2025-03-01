﻿using Microsoft.Data.SqlClient;
using ShoesShop.Models;
using System.Data;

namespace ShoesShop.Data
{
    public class CartRepository
    {
        private readonly string _connectionString;

        public CartRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public IEnumerable<CartModel> GetCartByUserId(string userId)
        {
            var carts = new List<CartModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_CartItems_SelectByUserId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    carts.Add(new CartModel
                    {
                        CartItemId = Convert.ToInt32(reader["CartItemId"]),
                        UserId = reader["UserId"].ToString(),
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ShoeName = reader["ShoeName"].ToString(),
                        ShoePrice = Convert.ToDecimal(reader["ShoePrice"]),
                        ShoeImageURL = reader["ShoeImageURL"].ToString()
                    });
                }
            }
            return carts;
        }

        public bool Delete(int cartId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_CartItems_DeleteById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CartItemId", cartId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Insert(AddCartModel cart)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_CartItems_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", cart.UserId);
                cmd.Parameters.AddWithValue("@ShoeId", cart.ShoeId);
                cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool IsItemInCart(string userId, int shoeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("CheckItemInCart", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);

                conn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar()); // Will return 1 (exists) or 0 (does not exist)
                return result == 1;
            }
        }

    }
}
