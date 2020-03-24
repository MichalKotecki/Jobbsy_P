using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Jobbsy.Models
{
    public class Photo
    {
        [Key]
        public int photoID { get; set; }

        public string path { get; set; }

    }
}
