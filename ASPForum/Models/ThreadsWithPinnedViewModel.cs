using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPForum.Models
{
    public class ThreadsWithPinnedViewModel
    {
        public IEnumerable<Thread> UnpinnedThreads { get; set; }
        public IEnumerable<Thread> PinnedThreads { get; set; }
    }
}