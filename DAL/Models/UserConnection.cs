using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class UserConnection
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long ConnectedUserId { get; set; }

        [Required]
        public bool IsFavorited { get; set; }

        [Required]
        public bool IsBlocked { get; set; }
    }
}
