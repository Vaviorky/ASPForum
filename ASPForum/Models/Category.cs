using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASPForum.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<ApplicationUser> Moderators { get; set; }


    }
}