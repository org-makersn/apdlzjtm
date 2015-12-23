using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PRINTER")]
    public class StorePrinterT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("PRINTER_NAME")]
        public virtual string PrinterName { get; set; }
        [Column("SIZE_X")]
        public virtual float SizeX { get; set; }
        [Column("SIZE_Y")]
        public virtual float SizeY { get; set; }
        [Column("SIZE_Z")]
        public virtual float SizeZ { get; set; }
        [Column("PRINTING_TYPE_NO")]
        public virtual int PrintingTypeNo { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
    }
}
