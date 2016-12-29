using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class MessageViewModel
    {
        public Message Message { get; set; }
        public ApplicationUser User { get; set; }
    }
}