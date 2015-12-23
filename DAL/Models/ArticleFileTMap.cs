using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class ArticleFileTMap : ClassMap<ArticleFileT>
    {
        public ArticleFileTMap()
        {
            Table("ARTICLE_FILE");

            Id(x => x.No, "NO");
            Map(x => x.FileGubun, "FILE_GUBUN");
            Map(x => x.FileType, "FILE_TYPE");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.Seq, "SEQ");
            Map(x => x.ImgUseYn, "IMG_USE_YN");
            Map(x => x.Ext, "EXT");
            Map(x => x.ThumbYn, "THUMB_YN");
            Map(x => x.MimeType, "MIME_TYPE");
            Map(x => x.Name, "NAME");
            Map(x => x.Size, "SIZE");
            Map(x => x.Rename, "RENAME");
            Map(x => x.ImgName, "IMG_NAME");
            Map(x => x.Path, "PATH");
            Map(x => x.Width, "WIDTH");
            Map(x => x.Height, "HEIGHT");
            Map(x => x.UseYn, "USE_YN");
            Map(x => x.Temp, "TEMP");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");

            Map(x => x.X, "X_SIZE");
            Map(x => x.Y ,"Y_SIZE");
            Map(x => x.Z, "Z_SIZE");
            Map(x => x.Volume, "VOLUME");
            Map(x => x.PrintVolume, "PRINT_VOLUME");
        }
    }
}
