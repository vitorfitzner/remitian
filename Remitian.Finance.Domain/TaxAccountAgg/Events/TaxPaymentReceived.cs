using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remitian.Finance.Domain.TaxAccountAgg.Events
{
    public class TaxPaymentReceived : Event
    {
        public int TaxAccountId { get; init; }
        public int BankAccountOriginId { get; init; }
        public int AmountCents { get; init; }
    }
}
