using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}