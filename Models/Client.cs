using System.ComponentModel.DataAnnotations;

namespace AtlanticQiAPI.Models
{
    public class Client
    {
        [Key]
        public int idClient { get; set; }

        [Required]
        [MaxLength(50)]
        public string firstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string lastName { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public string phone { get; set; }

        public DateTime birthDate { get; set; }

        public string documentType { get; set; }

        [Required]
        public string documentNumber { get; set; }

        public string address { get; set; }

        public string city { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;

        public DateTime? updatedAt { get; set; }
    }
}
