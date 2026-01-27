using System.ComponentModel.DataAnnotations;

namespace tunepool.Repository.Model
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime? createdAt { get; set; }

    }
}
