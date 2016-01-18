using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    //[Table("STORE_MATERIAL")]
    //public class StoreMaterialT
    //{
    //    [Key]
    //    [Column("NO")]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public virtual int No { get; set; }

    //    [Column("NAME")]
    //    public virtual string Name { get; set; }
    //    [Column("PRICE")]
    //    public virtual int Price { get; set; }
    //    [Column("SLICE_YN")]
    //    public virtual string SliceYn{ get; set; }
    //    [Column("IMAGE_NAME")]
    //    public virtual string ImageName { get; set; }
    //    [Column("SORT")]
    //    public virtual int Sort { get; set; }
    //    [Column("REG_DT")]
    //    public virtual DateTime? RegDt { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //    [Column("UPD_DT")]
    //    public virtual DateTime? UpdDt { get; set; }
    //    [Column("UPD_ID")]
    //    public virtual string UpdId { get; set; }
    //    //[Column("PRINTER_NO")]
    //    //public virtual int PrinterNo { get; set; }

    //}


    [Table("STORE_MATERIAL")]
    public partial class StoreMaterialT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public string NAME { get; set; }
        public int PRICE { get; set; }
        public string SLICE_YN { get; set; }
        public string IMAGE_NAME { get; set; }
        public int SORT { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<System.DateTime> UPD_DT { get; set; }
        public string UPD_ID { get; set; }
    }
}
