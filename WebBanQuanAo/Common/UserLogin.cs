using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo
{
    [Serializable]
    public class UserLogin
    { 
        public long UserID { get; set; }
        public string UserName { get; set; }

        public string GroupID { set; get; }
    }
}