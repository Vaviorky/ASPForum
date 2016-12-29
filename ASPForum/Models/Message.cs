using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ASPForum.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Title { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public bool IsRead { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual ICollection<MessageUser> Users { get; set; }
    }
}