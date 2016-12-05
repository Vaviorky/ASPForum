using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace ASPForum.Models
{
    public class Friends
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string FriendId { get; set; }
        public virtual ApplicationUser Friend { get; set; }
    }
}