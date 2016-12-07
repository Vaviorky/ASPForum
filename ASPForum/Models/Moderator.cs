﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class Moderator
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Subject Subject { get; set; }
    }
}