using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Breed, OwnerId, Notes, ImageUrl
                        FROM Dog
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null :
                                    reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null :
                                       reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }


        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Breed, OwnerId, Notes, ImageUrl
                        FROM Dog
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("id", id);

                    Dog dog = null;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null :
                                    reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null :
                                       reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };
                    }

                    reader.Close();
                    return dog;
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Dog ([Name], Breed, OwnerId, Notes, ImageUrl)
                        OUTPUT INSERTED.ID
                        VALUES (@name, @breed, @ownerId, @notes, @imageUrl)
                    ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@notes", string.IsNullOrEmpty(dog.Notes) ? (object)DBNull.Value : dog.Notes);
                    cmd.Parameters.AddWithValue("@imageUrl", string.IsNullOrEmpty(dog.ImageUrl) ? (object)DBNull.Value : dog.ImageUrl);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }


        public void UpdateDog(Dog dog)
        {
            return;
        }

        public void DeleteDog(int id)
        {
            return;
        }



    }
}
