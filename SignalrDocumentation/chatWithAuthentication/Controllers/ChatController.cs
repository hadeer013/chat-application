using AutoMapper;
using ChatWithAuth.DAL.Data;
using chatWithAuthentication.Hubs;
using chatWithAuthentication.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chatWithAuthentication.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ChatController(ApplicationContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.context = context;

            this.userManager = userManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var users = context.Users.ToList();
            return View(users);
        }
        [Authorize]
        public async Task<IActionResult> ChatPrivate(string ReceiverId)
        {
            var CurrentUser = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var user = await context.Users.FindAsync(ReceiverId);
            if (user != null)
            {
                var msgs = context.Messages.Where(m => (m.SenderUserId == CurrentUser.Id && m.ReceiverUserId == ReceiverId) || (m.ReceiverUserId == CurrentUser.Id && m.SenderUserId == ReceiverId)).OrderBy(m => m.date).ToList();
                var MappedMegs = mapper.Map<IEnumerable<MessageViewModel>>(msgs);
                foreach (var message in MappedMegs)
                {
                    if (CurrentUser.UserName == message.SenderName)
                        message.IsSent = true;
                }
                ViewData["RecieverName"] = user.UserName;
                return View(MappedMegs);
            }
            return NotFound();

        }
        [Authorize]
        public async Task<IActionResult> ChatPrivateTrial(string ReceiverId)
        {
            var CurrentUser = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var user = await context.Users.FindAsync(ReceiverId);
            if (user != null)
            {
                var msgs = context.Messages.Where(m => (m.SenderUserId == CurrentUser.Id && m.ReceiverUserId == ReceiverId) || (m.ReceiverUserId == CurrentUser.Id && m.SenderUserId == ReceiverId)).OrderBy(m => m.date).ToList();
                var MappedMegs = mapper.Map<IEnumerable<MessageViewModel>>(msgs);
                foreach (var message in MappedMegs)
                {
                    if (CurrentUser.UserName == message.SenderName)
                        message.IsSent = true;
                }
                ViewData["RecieverName"] = user.UserName;
                return View(MappedMegs);
            }
            return NotFound();
        }
    }
}
