// Models/Confirmation.cs
using System.ComponentModel.DataAnnotations;

namespace MVCBasicCurd.Models
{
    public enum ConfirmationType
    {
        KYCVerification,
        TransactionOTP,
        AccountActivation,
        PasswordReset
    }

    public enum ConfirmationStatus
    {
        Pending,
        Confirmed,
        Expired,
        Failed
    }

    public class Confirmation
    {
        [Key]
        public Guid ConfirmationId { get; set; } = Guid.NewGuid();

        [Required]
        public ConfirmationType Type { get; set; }

        /// <summary>
        /// Reference to account if confirmation relates to account operations; else null.
        /// </summary>
        [MaxLength(32)]
        public string? AccountNumber { get; set; }

        /// <summary>
        /// OTP or token (store hashed in production).
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string TokenHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }

        [Required]
        public ConfirmationStatus Status { get; set; } = ConfirmationStatus.Pending;

        [MaxLength(200)]
        public string? Metadata { get; set; }  // e.g. IP, device, reason
    }
}
