using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PRINTING_COMPANY")]
    public class StorePrintingCompanyT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("NAME")]
        public virtual string Name { get; set; }
        [Column("OFFICE_PHONE")]
        public virtual string OfficePhone { get; set; }
        [Column("ADDR1")]
        public virtual string Addr1 { get; set; }
        [Column("ADDR2")]
        public virtual string Addr2 { get; set; }
        [Column("POST_CODE")]
        public virtual string PostCode { get; set; }
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
