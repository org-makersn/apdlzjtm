using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    //[Table("STORE_PRINTING_COMPANY")]
    //public class StorePrintingCompanyT
    //{
    //    [Key]
    //    [Column("NO")]
    //    public virtual int No { get; set; }

    //    [Column("NAME")]
    //    public virtual string Name { get; set; }
    //    [Column("OFFICE_PHONE")]
    //    public virtual string OfficePhone { get; set; }
    //    [Column("ADDR1")]
    //    public virtual string Addr1 { get; set; }
    //    [Column("ADDR2")]
    //    public virtual string Addr2 { get; set; }
    //    [Column("POST_CODE")]
    //    public virtual string PostCode { get; set; }
    //    [Column("MANAGER_NAME")]
    //    public virtual string ManagerName { get; set; }
    //    [Column("URL")]
    //    public virtual string Url { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //    [Column("REG_DT")]
    //    public virtual DateTime RegDt { get; set; }
    //}

    [Table("STORE_PRINTING_COMPANY")]
    public partial class StorePrintingCompanyT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public string NAME { get; set; }
        public string OFFICE_PHONE { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string POST_CODE { get; set; }
        public string MANAGER_NAME { get; set; }
        public string URL { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }
}
