using System.ComponentModel.DataAnnotations;

namespace Infrastructure.SmsGate.DataTypes
{
    public class SendShortMessageCommand
    {
        [Required]
        public long SourceId { get; set; }

        [Required]
        public string Tenant { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Message { get; set; } = null!;
    }
}
