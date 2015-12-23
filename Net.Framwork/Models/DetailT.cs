using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.Models
{
    [Table("Detail")]
    public class DetailT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public virtual int DetailId { get; set; }

        [Column("Member_Id")]
        public virtual int MemberId { get; set; }
        [Column("Phone_Number")]
        public virtual string PhoneNumber { get; set; }
    }
}
