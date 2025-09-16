using System.ComponentModel.DataAnnotations;

namespace CleaningApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko klienta jest wymagane")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Adres jest wymagany")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Data zamówienia jest wymagana")]

        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Typ usługi jest wymagany")]
        public string ServiceType { get; set; }
        public string Status { get; set; }
    }
}
