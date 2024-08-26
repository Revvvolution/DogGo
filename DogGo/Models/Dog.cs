using Azure;
using System.ComponentModel;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int OwnerId {  get; set; }
        public string? Notes {  get; set; }  // Nullable in DB
        public string? ImageUrl {  get; set; }  // Nullable in DB
    }
}
