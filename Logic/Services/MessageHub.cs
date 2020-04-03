using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Models.Models;

namespace Logic.Services
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly UserManager<User> _userManager;

        // Connected IDs
        private static readonly IDictionary<string, User> UserTable = new ConcurrentDictionary<string, User>();

        public MessageHub(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
            
        public override async Task OnConnectedAsync()
        {
            UserTable[Context.ConnectionId] = await _userManager.FindByNameAsync(Context.User.Identity.Name);

            await Clients.All.SendAsync("log", "joined", Context.ConnectionId);
            await Clients.All.SendAsync("count", UserTable.Count);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            UserTable.Remove(Context.ConnectionId);

            await Clients.All.SendAsync("log", "left", Context.ConnectionId);
            await Clients.All.SendAsync("count", UserTable.Count);
        }
    }
}