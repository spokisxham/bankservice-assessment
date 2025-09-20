namespace BankAccountService.Contracts
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T eventMessage, string topicArn);
    }
}
