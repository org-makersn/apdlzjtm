using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class DropMemberT
    {
        public virtual int No { get; set; }
        [Required(ErrorMessage = "ID를 입력해주세요.")]
        public virtual string Id { get; set; }
        public virtual Nullable<int> Level { get; set; }
        public virtual string Name { get; set; }
        public virtual Nullable<System.DateTime> LastLoginDt { get; set; }
        public virtual string LastLoginIp { get; set; }
        public virtual Nullable<int> LoginCnt { get; set; }
        public virtual Nullable<System.DateTime> DelDt { get; set; }
        public virtual string DropComment { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual Nullable<System.DateTime> RegDt { get; set; }
        public virtual string RegId { get; set; }
    }
}
