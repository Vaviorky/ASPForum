using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace ASPForum.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}