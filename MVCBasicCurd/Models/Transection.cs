// Models/Transaction.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBasicCurd.Models
{
    public enum TransactionDirection
    {
        Credit, // money into the account
        Debit   // money out of the account
    }

    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Denormalized copy of the account number for quick queries and audit.
        /// FK relationship configured in DbContext.
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string AccountNumber { get; set; } = null!;

        /// <summary>
        /// Optional: store account holder name at the time of transaction (audit).
        /// </summary>
        [MaxLength(150)]
        public string? AccountHolderName { get; set; }

        /// <summary>
        /// Monetary amount for this transaction. Positive value.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionDirection Direction { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Balance after this transaction (denormalized snapshot).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        [MaxLength(250)]
        public string? Narration { get; set; }

        /// <summary>
        /// Id of related transaction in case of reversal / linked transactions.
        /// </summary>
        public Guid? RelatedTransactionId { get; set; }

        // Optionally link transaction to user (redundant but can be useful)
        public Guid? UserId { get; set; }

        // Indexes and FK configured on DbContext
    }
}
