using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
    //[Table("STORE_PRINTER_MATERIAL")]
    //public class StorePrinterMaterialT
    //{
    //    [Key]
    //    [Column("NO")]
    //    public virtual int No { get; set; }
    //    [Column("PRINTER_NO")]
    //    public virtual int PrinterNo { get; set; }
    //    [Column("MATERIAL_NO")]
    //    public virtual int MaterialNo { get; set; }
    //    [Column("REG_DT")]
    //    public virtual DateTime RegDt { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //    [Column("UPD_DT")]
    //    public virtual DateTime UpdDt { get; set; }
    //    [Column("UPD_ID")]
    //    public virtual string UpdId { get; set; }

    //    //public StorePrinterT Printer{get;set;}
    //    //public StoreMaterialT Material { get; set; }
    //}

    [Table("STORE_PRINTER_MATERIAL")]
    public partial class StorePrinterMaterialT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public int PRINTER_NO { get; set; }
        public int MATERIAL_NO { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<System.DateTime> UPD_DT { get; set; }
        public string UPD_ID { get; set; }
    }
}