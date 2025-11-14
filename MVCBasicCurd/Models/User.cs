// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace MVCBasicCurd.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        /// <summary>
        /// Business-facing unique id (e.g. USER-000123). Not the PK, but useful for display.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string BusinessUserId { get; set; } = null!;

        // Navigation: user can own multiple accounts
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        // Created / Updated timestamps (optional)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // inside Models/User.cs
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
