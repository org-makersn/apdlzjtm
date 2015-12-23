using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PRINTING_COM")]
    public class StorePrintingCompanyT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("NAME")]
        public virtual string Name { get; set; }
        [Column("PHONE_NUM")]
        public virtual string PhoneNum { get; set; }
        [Column("ADDR1")]
        public virtual string Addr1 { get; set; }
        [Column("ADDR2")]
        public virtual string Addr2 { get; set; }
        [Column("POST_NUM")]
        public virtual string PostNum { get; set; }
        [Column("MANAGER_NAME")]
        public virtual string ManagerName { get; set; }
        [Column("URL")]
        public virtual string Url { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
    }
}
