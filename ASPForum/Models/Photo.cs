using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        [Required]
        public virtual Post Post { get; set; }

    }
}