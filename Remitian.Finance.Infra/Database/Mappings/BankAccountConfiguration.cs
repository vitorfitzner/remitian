using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Remitian.Finance.Domain.AccountAgg;

namespace Remitian.Finance.Infra.Database.Mappings
{
    internal class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("BankAccounts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Ignore(x => x.BalanceCents);

            builder.HasMany(x => x.Transactions)
                   .WithOne()
                   .HasForeignKey("BankAccountId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Transactions)
                   .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}