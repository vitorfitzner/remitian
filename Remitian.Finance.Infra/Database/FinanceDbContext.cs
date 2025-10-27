using Microsoft.EntityFrameworkCore;
using Remitian.Finance.Domain.AccountAgg;
using Remitian.Finance.Infra.Database.Mappings;

namespace Remitian.Finance.Infra.Database
{
    // Define the DbContext for Account
    public class FinanceDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
