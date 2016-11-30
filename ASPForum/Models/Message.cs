using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}