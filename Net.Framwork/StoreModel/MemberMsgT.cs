using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
	[Table("MEMBER_MSG")]
	public class MemberMsgT
	{
		[Key]
		[Column("NO")]
		public virtual long No { get; set; }

		[Column("ROOM_NAME")]
		public virtual string RoomName { get; set; }
		[Column("MEMBER_NO")]
		public virtual int MemberNo { get; set; }
		[Column("MEMBER_NO_REF")]
		public virtual int MemberNoRef { get; set; }
		[Column("MSG_GUBUN")]
		public virtual string MsgGubun { get; set; }
		[Column("COMMENT")]
		public virtual string Comment { get; set; }
		[Column("IS_NEW")]
		public virtual string IsNew { get; set; }
		[Column("DEL_FLAG")]
		public virtual string DelFlag { get; set; }
		[Column("REG_IP")]
		public virtual string RegIp { get; set; }
		[Column("REG_DT")]
		public virtual DateTime RegDt { get; set; }
		[Column("REG_ID")]
		public virtual string RegId { get; set; }
		[Column("SITE_GUBUN")]
		public virtual string SiteGubun { get; set; }
	}
}
