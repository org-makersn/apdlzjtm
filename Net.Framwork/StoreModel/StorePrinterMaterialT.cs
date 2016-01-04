using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
    [Table("STORE_PRINTER_MATERIAL")]
    public class StorePrinterMaterialT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }
        [Column("PRINTER_NO")]
        public virtual int PrinterNo { get; set; }
        [Column("MATERIAL_NO")]
        public virtual int MaterialNo { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual DateTime UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }

        //public StorePrinterT Printer{get;set;}
        //public StoreMaterialT Material { get; set; }
    }

}



//[Table("STORE_NOTICE")]
//public class StoreNoticeT
//{
//    [Key]
//    [Column("NO")]
//    public virtual int No { get; set; }

//    [Column("PRODUCT_NO")]
//    public virtual int ProductNo { get; set; }
//    [Column("MEMBER_NO")]
//    public virtual int MemberNo { get; set; }
//    [Column("MEMBER_NO_REF")]
//    public virtual int MemberNoRef { get; set; }
//    [Column("REF_NO")]
//    public virtual int RefNo { get; set; }
//    [Column("TYPE")]
//    public virtual string Type { get; set; }
//    [Column("CONTENT")]
//    public virtual string Content { get; set; }
//    [Column("CHECK_YN")]
//    public virtual string CheckYn { get; set; }
//    [Column("IS_NEW_YN")]
//    public virtual string IsNewYn { get; set; }
//    [Column("REG_DT")]
//    public virtual DateTime RegDt { get; set; }
//    [Column("REG_ID")]
//    public virtual string RegId { get; set; }
//}