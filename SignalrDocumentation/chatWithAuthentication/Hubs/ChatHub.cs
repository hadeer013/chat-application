
using ChatWithAuth.DAL.Data;
using chatWithAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatWithAuthentication.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        private readonly static Dictionary<string, string> connectionMap = new Dictionary<string, string>();
        private readonly static List<ApplicationUser> OnlineUsers=new List<ApplicationUser>();
        private readonly ApplicationContext context;

        public ChatHub(ApplicationContext context)
        {
            this.context = context;
        }
        public async Task SendMessagePrivate(string ReceiverName,string Message)
        {
            var Receiver = context.Users.Where(s => s.UserName == ReceiverName).First();
            var Sender = context.Users.Where(s => s.UserName == IdentityName()).First();
            var msg = new Message()
            {
                Contetnt = Message,
                ReceiverUserId = Receiver.Id,
                SenderUserId = Sender.Id
            };
            
            context.Messages.Add(msg);
            await context.SaveChangesAsync();
            var msgViewModel = new MessageViewModel()
            {
                Id = msg.Id,
                Contetnt = msg.Contetnt,
                date = msg.date,
                SenderName = Sender.UserName,
                RecieverName=Receiver.UserName,
                IsSent =true
            };
            if (connectionMap.TryGetValue(ReceiverName, out string ConnectionId))
            {
                await Clients.Client(ConnectionId).SendAsync("NewMsg", msg.Contetnt,msg.date, Sender.UserName);
                await Clients.Caller.SendAsync("NewMsgSender", msg.Contetnt, msg.date,Sender.UserName);
            }
            else
              await  Clients.Caller.SendAsync("NewMsgSender", msg.Contetnt, msg.date, Sender.UserName);
        }
        public override Task OnConnectedAsync()
        {
            try
            {
                var userName = IdentityName();
                var user=context.Users.Where(s=>s.UserName==userName).FirstOrDefault();
                if(!OnlineUsers.Any(s=>s.UserName==userName)) {
                    OnlineUsers.Add(user);
                    connectionMap.Add(userName, Context.ConnectionId);
                }
                
            }
            catch (Exception e)
            {
                Clients.Caller.SendAsync("ErrorMsg", "Error occur while connecting to hub : "+e.Message);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = OnlineUsers.Where(s => s.UserName == IdentityName()).FirstOrDefault();
                OnlineUsers.Remove(user);
                connectionMap.Remove(IdentityName());
            }
            catch (Exception e)
            {
                Clients.Caller.SendAsync("ErrorMsg", "Error occur while disconnecting from hub : " + e.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }



        private string IdentityName()
            =>Context.User.Identity.Name;
    }
}
