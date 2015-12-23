using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_CATEGORY")]
    public class StoreCategoryT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("CATEGORY_NAME")]
        public virtual string CategoryNo { get; set; }
        [Column("CODE")]
        public virtual int Code { get; set; }
        [Column("SORT")]
        public virtual int Sort { get; set; }
        [Column("PARENT_NO")]
        public virtual int ParentNo { get; set; }
        [Column("DEPTH")]
        public virtual int Depth { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }

    }

}
