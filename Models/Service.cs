using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleaningApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa usługi jest wymagana")]
        [StringLength(100)]
        public string Name { get; set; } = null!; 

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DefaultPrice { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public string? ImageUrl { get; set; }
        public string? EstimatedTime { get; set; } 
        public string? AreaRange { get; set; }
        public string? Includes { get; set; }
    }
}