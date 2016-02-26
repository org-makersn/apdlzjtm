using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design.Web.Front.Models
{
    public class AccountModels
    {
        public class LogOnModel
        {
            [Required]
            [Display(Name = "User name")]
            public string UserId { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "RememberMe")]
            public bool RememberMe { get; set; }

            [Required]
            [Display(Name = "RememberPwd")]
            public bool RememberPwd { get; set; }
        }

        public class LogOnPageModel
        {
            public string ReturlUrl { get; set; } //랜딩할 페이지
        }

        public class JoinOnModel
        {
            [Required]
            [Display(Name = "User name")]
            public string JoinName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string JoinPassword { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string JoinEmail { get; set; }

        }

        public class RegisterExternalLoginModel
        {
            [Required]
            [Display(Name = "사용자 이름")]
            public string UserName { get; set; }

            public string ExternalLoginData { get; set; }
        }

        public class ExternalLogin
        {
            public string Provider { get; set; }
            public string ProviderDisplayName { get; set; }
            public string ProviderUserId { get; set; }
        }

        public class QnaModel
        {
            public int QnACode { get; set; }
            public string Email { get; set; }
            public string Title { get; set; }
            public string Comment { get; set; }
        }
    }
}
