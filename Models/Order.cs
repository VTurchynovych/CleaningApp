using System.ComponentModel.DataAnnotations;

namespace CleaningApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Imię i nazwisko klienta jest wymagane")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Adres jest wymagany")]

        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Data zamówienia jest wymagana")]

        public string ServiceType { get; set; }
        [Required(ErrorMessage = "Typ usługi jest wymagany")]
        public OrderStatus Status { get; set; }

    }
}

