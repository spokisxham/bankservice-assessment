namespace BankAccountService.DTOs
{
    public class WithdrawalRequestDto
    {
        public long AccountId { get; set; }
        public decimal Amount
        {
            get; set;
        }
    }
}
