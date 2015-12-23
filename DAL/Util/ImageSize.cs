using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Util
{
    public class ImageSize
    {
        /// <summary>
        /// 배너 이미지 사이즈
        /// </summary>
        /// <returns></returns>
        public IList<ResizeImg> GetBannerResize()
        {
            return new List<ResizeImg>()
            {
                //new ResizeImg(){width = 1000, height = 360, foldName = "Banner/fullsize"},
                new ResizeImg(){width = 100, height = 36, foldName = "Banner/thumb"}
            };
        }

        /// <summary>
        /// 게시글 이미지
        /// </summary>
        /// <returns></returns>
        public IList<ResizeImg> GetArticleResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 630, height = 470, foldName = "Article/article_img"},
                new ResizeImg(){width = 160, height = 120, foldName = "Article/article_img/thumb"}
            };
        }



        public IList<ResizeImg> GetMsgFIleResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 160, height = 90, foldName = "Msg_File/thumb"},
            };
        }

        /// <summary>
        /// 프로필 이미지
        /// </summary>
        /// <returns></returns>
        public IList<ResizeImg> GetProfileResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 160, height = 120, foldName = "Profile/thumb"}
            };
        }

        /// <summary>
        /// Cover 이미지
        /// </summary>
        /// <returns></returns>
        public IList<ResizeImg> GetCoverResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 1000, height = 400, foldName = "Profile/thumb"}
            };
        }

        public IList<ResizeImg> GetPrinterResize() { 
            return new List<ResizeImg>(){
                new ResizeImg(){width = 630,height=470,foldName="Printer/printer_img"},
                new ResizeImg(){width = 160,height=120,foldName="Printer/printer_img/thumb"}
            };
        }
    }
}
