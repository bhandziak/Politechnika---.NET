using EventRegisterProject.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace EventRegisterProject.Models
{
    public class Event
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [FutureDate(ErrorMessage = "Data musi być w przyszłości")]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
