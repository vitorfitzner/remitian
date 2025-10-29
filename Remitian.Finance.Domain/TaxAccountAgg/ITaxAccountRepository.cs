using System.Threading.Tasks;

namespace Remitian.Finance.Domain.TaxAccountAgg
{
    public interface ITaxAccountRepository
    {   
        Task<TaxAccount> GetTaxAccountByIdAsync(int taxAccountId);
        Task Save(TaxAccount taxAccount);
    }
}
