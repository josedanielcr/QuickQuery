using Microsoft.AspNetCore.SignalR;

namespace AutocompleteServiceAPI.Hubs
{
    public sealed class AutocompleteHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        }
    }
}
