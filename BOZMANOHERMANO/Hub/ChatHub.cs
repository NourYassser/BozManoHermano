using Microsoft.AspNetCore.SignalR;

namespace BOZMANOHERMANO.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;
            await Clients.Group(receiverId).SendAsync("ReceiveMessage", new
            {
                senderId,
                message,
                sentAt = DateTime.UtcNow
            });
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await base.OnConnectedAsync();
        }
    }
}
