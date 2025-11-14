// DTOs/StatementDto.cs  (not an EF entity; used for return)
namespace MVCBasicCurd.DTOs
{
    public class StatementDto
    {
        public string AccountNumber { get; set; } = null!;
        public string AccountHolder { get; set; } = null!;
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<TransactionLineDto> Transactions { get; set; } = Array.Empty<TransactionLineDto>();
    }

    public class TransactionLineDto
    {
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Narration { get; set; }
        public decimal Amount { get; set; }
        public string Direction { get; set; } = null!;
        public decimal BalanceAfter { get; set; }
    }
}
