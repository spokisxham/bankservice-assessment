namespace BankAccountService.DTOs
{
    public class WithdrawalEventDto
    {
        public decimal Amount { get; set; }
        public long AccountId { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
