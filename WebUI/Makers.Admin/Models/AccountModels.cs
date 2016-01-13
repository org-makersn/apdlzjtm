﻿using System.ComponentModel.DataAnnotations;

namespace Makers.Admin.Models
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
    }
}
