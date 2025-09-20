using BankAccountService.Contracts;
using BankAccountService.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BankAccountService.Services
{

    public class BankAccountService : IBankAccountService
    {
        private readonly SqlConnection _dbConnection;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<BankAccountService> _logger;
        private readonly string _snsTopicArn;

        public BankAccountService(SqlConnection dbConnection, IEventPublisher eventPublisher, ILogger<BankAccountService> logger)
        {
            _dbConnection = dbConnection;
            _eventPublisher = eventPublisher;
            _logger = logger;
            // The topic ARN should be loaded from configuration, and not  hard-coded.
            _snsTopicArn = "arn:aws:sns:YOUR_REGION:YOUR_ACCOUNT_ID:YOUR_TOPIC_NAME";
        }

        /// <summary>
        /// Handles the withdrawal logic.
        /// </summary>
        public async Task<string> WithdrawAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
            {
                _logger.LogWarning("Invalid withdrawal amount for account {AccountId}", accountId);
                return "Invalid withdrawal amount";
            }

            try
            {
                await _dbConnection.OpenAsync();
                using (var transaction = _dbConnection.BeginTransaction(IsolationLevel.Serializable))
                {

                    string sqlCheckBalance = "SELECT balance FROM accounts WITH (UPDLOCK) WHERE id = @id";
                    using (var cmdCheck = new SqlCommand(sqlCheckBalance, (SqlConnection)_dbConnection, (SqlTransaction)transaction))
                    {
                        cmdCheck.Parameters.Add(new SqlParameter("@id", accountId));
                        var currentBalance = (decimal?)await cmdCheck.ExecuteScalarAsync();

                        if (currentBalance == null || currentBalance < amount)
                        {
                            transaction.Rollback();
                            return "Insufficient funds for withdrawal";
                        }
                    }

                    string sqlUpdateBalance = "UPDATE accounts SET balance = balance - @amount WHERE id = @id";
                    using (var cmdUpdate = new SqlCommand(sqlUpdateBalance, (SqlConnection)_dbConnection, (SqlTransaction)transaction))
                    {
                        cmdUpdate.Parameters.Add(new SqlParameter("@amount", amount));
                        cmdUpdate.Parameters.Add(new SqlParameter("@id", accountId));
                        int rowsAffected = await cmdUpdate.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            await _eventPublisher.PublishAsync(new WithdrawalEventDto
                            {
                                AccountId = accountId,
                                Amount = amount,
                                Status = "SUCCESSFUL"
                            }, _snsTopicArn);
                            return "Withdrawal successful";
                        }
                        else
                        {
                            transaction.Rollback();
                            return "Withdrawal failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during withdrawal for account {AccountId}", accountId);
                return "Withdrawal failed due to system error";
            }
            finally
            {
                _dbConnection.Close();
            }
        }
    }

}
