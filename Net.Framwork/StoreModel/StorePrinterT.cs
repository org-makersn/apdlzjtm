using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    //[Table("STORE_PRINTER")]
    //public class StorePrinterT
    //{
    //    [Key]
    //    [Column("NO")]
    //    public virtual int No { get; set; }

    //    [Column("NAME")]
    //    public virtual string Name { get; set; }
    //    [Column("SIZE_X")]
    //    public virtual double SizeX { get; set; }
    //    [Column("SIZE_Y")]
    //    public virtual double SizeY { get; set; }
    //    [Column("SIZE_Z")]
    //    public virtual double SizeZ { get; set; }
    //    [Column("PRINTING_COMPANY_NO")]
    //    public virtual int PrintingCompanyNo { get; set; }
    //    [Column("PRINTING_TYPE_NO")]
    //    public virtual int PrintingTypeNo { get; set; }

    //    [Column("REG_DT")]
    //    public virtual DateTime RegDt { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //    [NotMapped]
    //    public virtual IList<StoreMaterialT> materials { get; set; }  
    //}

    [Table("STORE_PRINTER")]
    public partial class StorePrinterT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public string NAME { get; set; }
        public double SIZE_X { get; set; }
        public double SIZE_Y { get; set; }
        public double SIZE_Z { get; set; }
        public int PRINTING_COMPANY_NO { get; set; }
        public int PRINTING_TYPE_NO { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }
}