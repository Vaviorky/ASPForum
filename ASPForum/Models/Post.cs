using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ASPForum.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Thread Post { get; set; }
    }
}