using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AdListing
{
   
        public class ManageAds
        {
            private string _conn;
            
        public ManageAds(string conn)
            {
                _conn = conn;
            }

            public List<AdPost> GetPosts()
            {
                List<AdPost> Ads = new List<AdPost>();

                using (SqlConnection conn = new SqlConnection(_conn))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM AdPosts order by DatePosted desc";
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Ads.Add(new AdPost
                        {
                            Id = (int)reader["Id"],
                            DatePosted = (DateTime)reader["DatePosted"],
                            Text = (String)reader["Text"],
                            Name = reader.GetOrNull<String>("Name"),
                            Phone = (string)reader["Phone"]
                        });

                    }
                }
                return Ads;
            }

            public void AddNewAd(AdPost newAd)
            {

                using (SqlConnection conn = new SqlConnection(_conn))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO AdPosts (text, name, phone) SELECT @text, @name, @phone
                                   SELECT SCOPE_IDENTITY()";
                    cmd.Parameters.AddWithValue("@text", newAd.Text);
                    cmd.Parameters.AddWithValue("@phone", newAd.Phone);
                    if (String.IsNullOrEmpty(newAd.Name))
                    {
                     cmd.Parameters.AddWithValue("@name", DBNull.Value);
                    }
                    else
                   {
                    cmd.Parameters.AddWithValue("@name",newAd.Name);
                   };

                conn.Open();

                    newAd.Id = (int)(decimal)cmd.ExecuteScalar();
                }
            
           
            }

            public void DeletePost(int postId)
            {
                using (SqlConnection conn = new SqlConnection(_conn))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM AdPosts Where Id = @id";
                    cmd.Parameters.AddWithValue("@id", postId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        
    }

    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string column)
        {
            object obj = reader[column];
            if (obj == DBNull.Value)
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}
