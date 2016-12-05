using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}