using System.ComponentModel.DataAnnotations;

namespace Infrastructure.EmailGate.DataTypes
{
    public class SendEmailMessageCommand
    {
        [Required]
        public long SourceId { get; set; }

        [Required]
        public string Tenant { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string Message { get; set; } = null!;
    }
}
