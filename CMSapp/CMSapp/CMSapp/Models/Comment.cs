using System;
using System.Collections.Generic;
using System.Text;

namespace CMSapp.Models
{
    public class Comment
    {
        public string applicationid { get; set; }
        public string commenter { get; set; }
        public string comment_by { get; set; }
        public DateTime comment_time { get; set; }
        public string comment { get;set; }
    }
}
