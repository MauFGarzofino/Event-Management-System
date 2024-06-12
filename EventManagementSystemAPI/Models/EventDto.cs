using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementSystemAPI.Models
{
    public class EventDto
    {
        [Required]
        public string? Title { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Column(TypeName = "Time")]
        public TimeSpan Time { get; set; }

        [Required]
        public string? Location { get; set; }
    }
}
