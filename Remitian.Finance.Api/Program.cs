using Microsoft.EntityFrameworkCore;
using Remitian.Finance.Api.Hubs;
using Remitian.Finance.Api.Resources.BankAccountResource;
using Remitian.Finance.Domain.AccountAgg;
using Remitian.Finance.Infra.Database;
using Remitian.Finance.Infra.Database.Repositories;

namespace Remitian.Finance.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(_ => true); // permite qualquer origem
                });
            });


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSignalR();

            builder.Services.AddDbContext<FinanceDbContext>(opt =>
                opt.UseInMemoryDatabase("FinanceDb"));

            builder.Services.AddScoped<BankAccountService>();
            builder.Services.AddScoped<BankAccountRepository>();

            var app = builder.Build();

            app.UseCors("AllowAll");

            app.MapHub<NotificationsHub>("/hubs/notifications");

            SeedDatabase(app);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void SeedDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();

            db.Database.EnsureCreated();

            if (!db.BankAccounts.Any())
            {
                db.BankAccounts.AddRange(
                    new BankAccount { Id = 1, Name = "Vitor" },
                    new BankAccount { Id = 2, Name = "PO" },
                    new BankAccount { Id = 3, Name = "Lucas" },
                    new BankAccount { Id = 4, Name = "Jamilson" }
                );
                db.SaveChanges();
            }
        }
    }
}
