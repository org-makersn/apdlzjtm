using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.Models
{
    [Table("Member")]
    public class MemberT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public virtual int MemberId { get; set; }

        [Required(ErrorMessage = "ID is must be Required.")]
        [Column("Member_Nm")]
        public virtual string MemberNm { get; set; }
        [Column("App_Id")]
        public virtual string AppId { get; set; }
        [Column("Reg_Dt")]
        public virtual DateTime RegDt { get; set; }

        //[Column("Phone_Number")]
        //public virtual string PhoneNumber { get; set; }
    }

    public class CustomMemberT
    {
        public virtual int MemberId { get; set; }
        public virtual string MemberNm { get; set; }
        public virtual string AppId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string PhoneNumber { get; set; }
    }
}