using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_MATERIAL")]
    public class StoreMaterialT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        //[Column("MEMBER_NO")]
        //public virtual int MemberNo { get; set; }
        [Column("NAME")]
        public virtual string Name { get; set; }
        [Column("PRICE")]
        public virtual int Price { get; set; }
        [Column("SLICE_YN")]
        public virtual string SliceYn{ get; set; }
        [Column("VIEW_IMG")]
        public virtual string ViewImg { get; set; }
        [Column("MAT_IMG")]
        public virtual string MatImg { get; set; }
        [Column("SORT")]
        public virtual int Sort { get; set; }
        [Column("REG_DT")]
        public virtual DateTime? RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual DateTime? UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
        //[Column("PRINTER_NO")]
        //public virtual int PrinterNo { get; set; }

    }
}
