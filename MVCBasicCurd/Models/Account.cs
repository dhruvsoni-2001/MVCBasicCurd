// Models/Account.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBasicCurd.Models
{
    public enum AccountType
    {
        Checking,
        Savings,
        Credit,
        Loan
    }

    public class Account
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Public bank account number, unique.
        /// Store as string to preserve leading zeros if any and support formatting.
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string AccountNumber { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }     // FK to User

        [ForeignKey(nameof(UserId))]
        public User? Owner { get; set; }

        [Required]
        public AccountType Type { get; set; }

        /// <summary>
        /// Current available balance. Use decimal(18,2) for currency.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0m;

        [MaxLength(100)]
        public string? NickName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Confirmation> Confirmations { get; set; } = new List<Confirmation>();
    }
}
