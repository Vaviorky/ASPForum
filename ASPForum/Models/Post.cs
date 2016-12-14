using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ASPForum.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        [AllowHtml]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int ThreadId { get; set; }
        public virtual Thread Thread { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

    }
}