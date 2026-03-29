using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Message
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long FromUserId { get; set; }

        [Required]
        public long ToUserId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DateSent { get; set; }
        
        [Required]
        public bool IsSeen { get; set; }
    }
}
