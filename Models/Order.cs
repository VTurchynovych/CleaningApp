using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApp.Models
{
    public class Order
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Klient jest wymagany")]
        public string ClientId { get; set; } = null!; 

        [ForeignKey("ClientId")]
        public virtual ApplicationUser Client { get; set; } = null!;

        public string? WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        public virtual ApplicationUser? Worker { get; set; }

        [Required(ErrorMessage = "Typ usługi jest wymagany")]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!; 

        [Required(ErrorMessage = "Adres jest wymagany")]
        public string Address { get; set; } = null!; 

        [Required(ErrorMessage = "Data zamówienia jest wymagana")]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
    }
}