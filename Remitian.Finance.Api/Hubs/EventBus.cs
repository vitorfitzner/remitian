using Microsoft.AspNetCore.SignalR;

namespace Remitian.Finance.Api.Hubs
{
    public class EventBus(IHubContext<NotificationsHub> hubContext)
    {
        public IHubContext<NotificationsHub> HubContext { get; } = hubContext;

        public async Task Publish(string method, object @event)
        {
            await HubContext.Clients.All.SendAsync(method, @event);
        }
    }
}
