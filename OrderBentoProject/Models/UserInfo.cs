using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderBentoProject.Models
{
    public class UserInfo
    {
        public int UserID { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string Name { get; set; }
        public bool Privilege { get; set; }
        public string UserPic { get; set; }
    }
}