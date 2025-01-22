using System.Text.Json.Serialization;
using BookDemo.Core.Models;

namespace BookDemo.Core.Entities
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public EntityStatus Status { get; set; }

    }
}
