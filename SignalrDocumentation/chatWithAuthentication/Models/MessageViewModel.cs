using ChatWithAuth.DAL.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace chatWithAuthentication.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Contetnt { get; set; }
        public DateTime date { get; set; } 
        public string SenderName { get; set; }
        public string RecieverName { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
