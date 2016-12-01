using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using System.ComponentModel.DataAnnotations;

namespace ASPForum.Models
{
    public class Thread
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        public DateTime Date { get; set; }
[Required]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}