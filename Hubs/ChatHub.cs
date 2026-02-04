using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp_With_RazorSignalR.Hubs
{
    public class ChatHub : Hub
    {
        // Thread-safe collection to store active users: ConnectionId -> Username
        private static ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

        public async Task JoinChat(string username)
        {
            _users.TryAdd(Context.ConnectionId, username);
            await Clients.All.SendAsync("UpdateUserList", _users.Values.ToList());
            await Clients.Others.SendAsync("ReceiveSystemMessage", $"{username} has joined the chat.");
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            if (_users.TryRemove(Context.ConnectionId, out var username))
            {
                await Clients.All.SendAsync("UpdateUserList", _users.Values.ToList());
                await Clients.Others.SendAsync("ReceiveSystemMessage", $"{username} has left the chat.");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendFile(string user, string fileName, string fileUrl, string fileType, long fileSize)
        {
            await Clients.All.SendAsync("ReceiveFile", user, fileName, fileUrl, fileType, fileSize);
        }

        public async Task SendTyping(string user)
        {
             await Clients.Others.SendAsync("UserTyping", user);
        }
    }
}
