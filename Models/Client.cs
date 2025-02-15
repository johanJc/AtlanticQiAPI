using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MaxLength(100)]
        public string? email { get; set; }

        [MaxLength(20)]
        public string? phone { get; set; }

        public DateTime? birthDate { get; set; }

        [MaxLength(20)]
        public string? documentType { get; set; }

        [Required]
        [MaxLength(30)]
        public string documentNumber { get; set; }

        [MaxLength(200)]
        public string? address { get; set; }

        [MaxLength(50)]
        public string? city { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;

        public DateTime? updatedAt { get; set; }        

        public int StatusId { get; set; } // Solo el ID

        public Status? StatusNavigation { get; set; }

    }
}
