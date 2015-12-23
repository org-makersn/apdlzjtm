using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design.Web.Front.Models
{
    public class ProfileModel
    {
        public virtual int UserNo { get; set; }
        public virtual string UserNm { get; set; }
        public virtual string UserId { get; set; }
        public virtual string UserProfilePic { get; set; }
        public virtual int UserLevel { get; set; }
    }
}
