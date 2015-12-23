using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAL.Models;

namespace Design.Web.Admin.Models
{
    public class UserContext
    {
        private static readonly string Key_Login_Member = "Admin_Login_Member";
        private static readonly MemberT anonymous = new MemberT();

        public UserContext()
        {
        }

        public MemberT Member
        {
            get
            {
                MemberT memberT = (MemberT)HttpContext.Current.Session[Key_Login_Member] != null ? (MemberT)HttpContext.Current.Session[Key_Login_Member] : anonymous;

                return memberT;
            }
            set
            {
                if (value == null)
                {
                    System.Web.HttpContext.Current.Session.Remove(Key_Login_Member);
                }
                else
                {
                    System.Web.HttpContext.Current.Session[Key_Login_Member] = value;
                }
            }
        }
    }
}
