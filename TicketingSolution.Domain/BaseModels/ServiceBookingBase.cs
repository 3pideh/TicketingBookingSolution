using System.ComponentModel.DataAnnotations;

namespace TicketingSolution.Domain.BaseModels
{
    public abstract class ServiceBookingBase
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        public string Family { get; set; }

        [Required]
        [StringLength(80)]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("Date Must Be In Future", new[] { nameof(Date) });
            }
        }
    }
}