using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    //[Table("STORE_PRINTING_TYPE")]
    //public class StorePrintingTypeT
    //{
    //    [Key]
    //    [Column("NO")]
    //    public virtual int No { get; set; }

    //    [Column("NAME")]
    //    public virtual string Name { get; set; }
    //    [Column("PRINTING_TYPE")]
    //    public virtual string PrintingType { get; set; }
    //    [Column("SLICE_YN")]
    //    public virtual char SliceYn { get; set; }
    //    [Column("REG_DT")]
    //    public virtual DateTime RegDt { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //}


    [Table("STORE_PRINTING_TYPE")]
    public partial class StorePrintingTypeT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public string NAME { get; set; }
        public string PRINTING_TYPE { get; set; }
        public string SLICE_YN { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }
}
