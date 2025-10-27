using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Remitian.Finance.Domain.AccountAgg;

namespace Remitian.Finance.Infra.Database.Mappings
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property<int>("BankAccountId");

            builder.Property(t => t.AmountCents)
                   .IsRequired();

            builder.Property(t => t.TransactionType)
                   .IsRequired();

            builder.Property(t => t.Date)
                   .IsRequired();

            builder.HasAnnotation("ConstructorBinding", true);
        }
    }
}