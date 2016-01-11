using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PRINTING_COMPANY_PRINTER")]
    public class StorePrintingCompanyPrinterT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("PRINTING_COMPANY_NO")]
        public virtual int PrintingCompanyNo { get; set; }
        [Column("PRINTER_NO")]
        public virtual int PrinterNo { get; set; }
        [Column("REG_DT")]
        public DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public string RegId { get; set; }
    }
}
