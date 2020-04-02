using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Interfaces;
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
        private static readonly IDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        private readonly Lazy<IStreamRipperManager> _streamRipperManager;

        public MessageHub(UserManager<User> userManager, Lazy<IStreamRipperManager> streamRipperManager)
        {
            _userManager = userManager;
            _streamRipperManager = streamRipperManager;
        }
            
        public override async Task OnConnectedAsync()
        {
            var user =  await _userManager.FindByNameAsync(Context.User.Identity.Name);

            Users.Add(Context.ConnectionId, user);

            await Clients.All.SendAsync("log", "joined", Context.ConnectionId);
            await Clients.All.SendAsync("count", Users.Count);

            await Clients.Client(Context.ConnectionId)
                .SendAsync("streams", await _streamRipperManager.Value.For(user).Status());
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Users.Remove(Context.ConnectionId);

            await Clients.All.SendAsync("log", "left", Context.ConnectionId);
            await Clients.All.SendAsync("count", Users.Count);
        }
    }
}