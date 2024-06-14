using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.DTOs
{
    public class UpdateEventDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [StringLength(200)]
        [SwaggerSchema(Description = "The description of the event")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [SwaggerSchema(Description = "The date of the event", Format = "date")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Column(TypeName = "Time")]
        [SwaggerSchema(Description = "The time of the event", Format = "time")]
        public TimeSpan Time { get; set; }

        [Required]
        [SwaggerSchema(Description = "The location of the event")]
        public string? Location { get; set; }
    }
}
