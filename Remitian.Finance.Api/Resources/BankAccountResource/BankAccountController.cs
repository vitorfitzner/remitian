using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Remitian.Finance.Api.Hubs;
using Remitian.Finance.Domain.AccountAgg;

namespace Remitian.Finance.Api.Resources.BankAccountResource
{
    /// <summary>
    /// Provides endpoints for managing bank accounts, including retrieving account information.
    /// </summary>
    /// <remarks>This controller handles HTTP requests related to bank accounts. It uses the <see
    /// cref="BankAccountService"/> to perform operations such as retrieving filtered lists of bank accounts.
    /// This class should handle only http-related logic, delegating business logic to the service layer.
    /// </remarks>
    /// <param name="service"></param>
    [ApiController]
    [Route("bankaccounts")]
    public class BankAccountController(BankAccountService service, IHubContext<NotificationsHub> hubContext) : ControllerBase
    {
        #region dependencies
        private readonly BankAccountService _service = service;
        private readonly IHubContext<NotificationsHub> _hub = hubContext;
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetBankAccounts([FromQuery] BankAccountFilter filter)
        {
            var bankAccounts = await _service.GetBankAccountsAsync(filter);

            await _hub.Clients.All.SendAsync("ReceiveMessage", 1, 100);
            
            await _hub.Clients.All.SendAsync("ReceiveNotification", "notification.Title", "notification.Message");

            return Ok(bankAccounts);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody]BankAccountDeposit deposit)
        {
            await _service.Deposit(deposit.BankAccountId, deposit.AmountCents);

            return Ok();
        }
    }
}
