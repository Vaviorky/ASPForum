using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        public string Text { get; set; }
        [Required]
        public virtual Category Category { get; set; }
        public virtual ICollection<Thread> Posts { get; set; }
    }
}