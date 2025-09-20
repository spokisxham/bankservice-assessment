using BankAccountService.Contracts;
using Newtonsoft.Json;

namespace BankAccountService.Services
{
 
    public class SnsEventPublisher : IEventPublisher
    {
        private readonly SnsClient _snsClient;
        private readonly ILogger<SnsEventPublisher> _logger;

        public SnsEventPublisher(SnsClient snsClient, ILogger<SnsEventPublisher> logger)
        {
            _snsClient = snsClient;
            _logger = logger;
        }

        /// <summary>
        /// Publishes a message to an SNS topic.
        /// </summary>
        public async Task PublishAsync<T>(T eventMessage, string topicArn)
        {
            try
            {
                var eventJson = JsonConvert.SerializeObject(eventMessage);
                var publishRequest = new PublishRequest
                {
                    Message = eventJson,
                    TopicArn = topicArn
                };
                await _snsClient.PublishAsync(publishRequest);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Failed to publish event to SNS topic {TopicArn}", topicArn);
               
            }
        }
    }
}
