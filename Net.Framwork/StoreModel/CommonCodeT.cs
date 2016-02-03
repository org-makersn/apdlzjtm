
namespace Net.Framework.StoreModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("COMMON_CODE")]
    public partial class CommonCodeT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public string CODE_GROUP { get; set; }
        public string CODE_TYPE { get; set; }
        public string CODE_KEY { get; set; }
        public string CODE_NAME { get; set; }
        public int IDX { get; set; }
        public string VISIBILITY { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }
}
