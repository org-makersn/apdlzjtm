using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Makersn.Models
{
    public class MemberT
    {
        public virtual int No { get; set; }
        [Required(ErrorMessage = "ID를 입력해주세요.")]
        public virtual string Id { get; set; }
        public virtual string BlogUrl { get; set; }
        public virtual int Level { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        public virtual string Status { get; set; }
        public virtual string Password { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string CellPhone { get; set; }
        public virtual string Url { get; set; }
        public virtual string SnsType { get; set; }
        public virtual string SnsId { get; set; }
        public virtual string ProfileMsg { get; set; }
        public virtual string ProfilePic { get; set; }
        public virtual string CoverPic { get; set; }
        public virtual string AllIs { get; set; }
        public virtual string RepleIs { get; set; }
        public virtual string LikeIs { get; set; }
        public virtual string NoticeIs { get; set; }
        public virtual Nullable<DateTime> UpdPasswordDt { get; set; }
        public virtual Nullable<DateTime> LastLoginDt { get; set; }
        public virtual string LastLoginIp { get; set; }
        public virtual Nullable<int> LoginCnt { get; set; }
        public virtual string emailCertify { get; set; }
        public virtual Nullable<DateTime> DelDt { get; set; }
        public virtual string DropComment { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual string FollowIs { get; set; }

        [IgnoreDataMember]
        public virtual int DownloadCnt { get; set; }
        [IgnoreDataMember]
        public virtual int UploadCntY { get; set; }
        [IgnoreDataMember]
        public virtual int UploadCntN { get; set; }
        [IgnoreDataMember]
        public virtual int CommentCnt { get; set; }
        [IgnoreDataMember]
        public virtual int OrderCnt { get; set; }
    }         
}             
              
              
              
              
              
              
              
              
              
              