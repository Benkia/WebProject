using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string Title { get; set; }
        public int FanID { get; set; }
        public Fan Fan { get; set; }
        public string SiteAddress { get; set; }
        public string Text { get; set; }
        public Post ReferencedPost { get; set; }
        public DateTime Date { get; set; }
    }
}