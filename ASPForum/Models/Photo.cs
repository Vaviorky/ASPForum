using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public virtual Post Post { get; set; }

    }
}