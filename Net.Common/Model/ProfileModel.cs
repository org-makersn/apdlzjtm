using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Common.Model
{
    public class ProfileModel
    {
        public virtual int UserNo { get; set; }
        public virtual string UserNm { get; set; }
        public virtual string UserId { get; set; }
        public virtual string UserProfilePic { get; set; }
        public virtual int UserLevel { get; set; }
        //public virtual bool IsStoreMember { get; set; }
    }
}
