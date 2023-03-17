using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWithAuth.DAL.Data
{
    public class ApplicationUser:IdentityUser
    {
        public ICollection<Message> ReceivedMessages { get; set; }=new HashSet<Message>();
        public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
    }
}
