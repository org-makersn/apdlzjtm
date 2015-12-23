using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class MemberTMap : ClassMap<MemberT>
    {
        public MemberTMap()
        {
            Id(x => x.No, "NO");
            Map(x => x.Id, "ID");
            Map(x => x.BlogUrl, "BLOG_URL");
            Map(x => x.Level, "LEVEL").Nullable();
            Map(x => x.Status, "STATUS").Nullable();
            Map(x => x.Password, "PASSWORD");
            Map(x => x.Name, "NAME");
            Map(x => x.Email, "EMAIL");
            Map(x => x.CellPhone, "CELL_PHONE");
            Map(x => x.Url, "URL");
            Map(x => x.SnsType, "SNS_TYPE");
            Map(x => x.SnsId, "SNS_ID");
            Map(x => x.ProfileMsg, "PROFILE_MSG");
            Map(x => x.ProfilePic, "PROFILE_PIC");
            Map(x => x.CoverPic, "COVER_PIC");
            Map(x => x.AllIs, "ALL_IS");
            Map(x => x.RepleIs, "REPLE_IS");
            Map(x => x.LikeIs, "LIKE_IS");
            Map(x => x.NoticeIs, "NOTICE_IS");
            Map(x => x.UpdPasswordDt, "UPD_PASSWORD_DT");
            Map(x => x.LastLoginDt, "LAST_LOGIN_DT");
            Map(x => x.LastLoginIp, "LAST_LOGIN_IP");
            Map(x => x.LoginCnt, "LOGIN_CNT");
            Map(x => x.emailCertify, "EMAIL_CERTIFY");
            Map(x => x.DelDt, "DEL_DT");
            Map(x => x.DropComment, "DROP_COMMENT");
            Map(x => x.DelFlag, "DEL_FLAG");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.FollowIs, "FOLLOW_IS");
            
            //Map(x => x.DownloadCnt, "DownloadCnt").Nullable();
            //Map(x => x.UploadCntY, "UploadCntY").Nullable();
            //Map(x => x.UploadCntN, "UploadCntN").Nullable();
            //Map(x => x.CommentCnt, "CommentCnt").Nullable();
            Table("MEMBER");
        }
    }
}
