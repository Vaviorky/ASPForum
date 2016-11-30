using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}