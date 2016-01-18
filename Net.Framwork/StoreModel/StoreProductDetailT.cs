using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
    //[Table("STORE_PRODUCT_DETAIL")]
    //public class StoreProductDetailT
    //{
    //    [Key]
    //    [Column("NO")]
    //    public virtual long No { get; set; }

    //    [Column("PRODUCT_NO")]
    //    public virtual long ProductNo { get; set; }
    //    [Column("MATERIAL_NO")]
    //    public virtual int MaterialNo { get; set; }
    //    [Column("TOTAL_PRICE")]
    //    public virtual int TotalPrice { get; set; }
    //    [Column("MARGIN_PRICE")]
    //    public virtual int MarginPrice { get; set; }
    //    [Column("BASE_PRICE")]
    //    public virtual int BasePrice { get; set; }
    //    [Column("DEFAULT_YN")]
    //    public virtual char DefaultYn { get; set; }
    //    [Column("PRINTED_YN")]
    //    public virtual char PrintedYn { get; set; }
    //    [Column("SUCCESS_RATE")]
    //    public virtual float SuccessRete { get; set; }
    //    [Column("SELECT_YN")]
    //    public virtual char SelectYn { get; set; }
    //    [Column("IMAGE_NAME")]
    //    public virtual string ImageName { get; set; }
    //    [Column("IMAGE_EXT")]
    //    public virtual string ImageExt { get; set; }
    //    [Column("SORT")]
    //    public virtual int Sort { get; set; }
    //    [Column("USE_YN")]
    //    public virtual char UseYn { get; set; }
    //    [Column("REG_DT")]
    //    public virtual DateTime RegDt { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //}

    [Table("STORE_PRODUCT_DETAIL")]
    public partial class StoreProductDetailT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long NO { get; set; }
        public long PRODUCT_NO { get; set; }
        public int MATERIAL_NO { get; set; }
        public int TOTAL_PRICE { get; set; }
        public int MARGIN_PRICE { get; set; }
        public int BASE_PRICE { get; set; }
        public string DEFAULT_YN { get; set; }
        public string PRINTED_YN { get; set; }
        public Nullable<double> SUCCESS_RATE { get; set; }
        public string SELECT_YN { get; set; }
        public string IMAGE_NAME { get; set; }
        public int SORT { get; set; }
        public string USE_YN { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }
}
