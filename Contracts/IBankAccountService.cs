namespace BankAccountService.Contracts
{
   
    public interface IBankAccountService
    {
        Task<string> WithdrawAsync(long accountId, decimal amount);
    }
}
