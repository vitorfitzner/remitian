using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Remitian.Finance.ApiClient.Services;

public sealed class RealtimeClient : IAsyncDisposable
{
    private readonly HubConnection _hub;

    public RealtimeClient(NavigationManager nav)
    {
        _hub = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:5000/hubs/notifications")) // Use the Uri overload
            .WithAutomaticReconnect()
            .Build();
    }

    public Task StartAsync() => _hub.StartAsync();

    public void OnBankAccountUpdated(Action<int, int> handler) =>
        _hub.On<int, int>("BankAccountUpdated", handler);

    public ValueTask DisposeAsync() => _hub.DisposeAsync();
}
