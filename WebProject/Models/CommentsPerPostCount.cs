using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.Models
{
    public class CommentsPerPostCount
    {
        public int PostID { get; set; }
        public string PostTitle { get; set; }
        public int Count { get; set; }
    }
}