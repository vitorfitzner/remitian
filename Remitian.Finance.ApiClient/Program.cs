using Remitian.Finance.ApiClient.Components;
using Remitian.Finance.ApiClient.Services;

namespace Remitian.Finance.ApiClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Register SignalR client helper
            builder.Services.AddScoped<RealtimeClient>();

            builder.Services
                    .AddCors(o => o.AddPolicy("frontend", p => p.WithOrigins("https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));

            var app = builder.Build();

            app.UseCors("frontend");

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
