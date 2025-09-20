using BankAccountService.Contracts;
using BankAccountService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountService.Controllers
{
 
    [Route("api/bank")]
    [ApiController]
    public class BankController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IBankAccountService _bankAccountService;

        public BankController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawalRequestDto request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Invalid withdrawal request.");
            }

            var result = await _bankAccountService.WithdrawAsync(request.AccountId, request.Amount);

            if (result == "Withdrawal successful")
            {
                return Ok(new { message = result });
            }
            else
            {
                return BadRequest(new { message = result });
            }
        }
    }
}
