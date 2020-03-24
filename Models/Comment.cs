using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jobbsy.Models
{
    public class Comment
    {
        public int commentID { get; set; }
        public int AspNetUsersID { get; set; }
        public int rate { get; set; }
        public String content { get; set; }
        public String date { get; set; }
    }
}
