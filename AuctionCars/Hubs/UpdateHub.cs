using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace AuctionCars.Hubs
{
    public class UpdateHub : Hub
    {
        public async Task JoinGroup(int group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.ToString());
        }
    }
}
