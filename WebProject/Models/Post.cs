using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebProject.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int FanID { get; set; }
        public Fan Fan { get; set; }
        public string SiteAddress { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string Picture { get; set; }
        public string Video { get; set; }
        private List<Comment> _comments;
        public List<Comment> Comments {
            get
            {
                if (_comments == null || _comments.Count == 0)
                {
                    _comments = new ApplicationDbContext().Comments.Include(c => c.Fan).ToList().FindAll(comment => comment.PostID == ID);
                }

                return _comments;
            }
            set
            {
                _comments = value;
            }
        }
    
    }
}