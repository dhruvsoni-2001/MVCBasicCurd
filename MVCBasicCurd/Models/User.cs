// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace MVCBasicCurd.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = null!;

        // Fixed syntax error here
        [MaxLength(32)]
        public string? AccountNumber { get; set; } // Stores the primary account number for easy display

        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Address { get; set; } = null!;

        [Required, MaxLength(50)]
        public string BusinessUserId { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}