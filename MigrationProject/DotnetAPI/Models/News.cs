using System;
using System.Collections.Generic;

namespace DotnetAPI.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public virtual User User { get; set; }
    }
}
