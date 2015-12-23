using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Design.Web.Front.Filter
{
    public class ContentFilter
    {
        public string UrlEncode(string str)
        {
            string sb = "";

            sb = HttpUtility.UrlEncode(str, Encoding.UTF8);
            return (sb);
        }

        public string UrlDecode(string urlStr)
        {
            HttpUtility.UrlDecode(urlStr, Encoding.UTF8);

            return (urlStr);
        }

        public string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
                        @"<script[^>]*?>.*?</script>",
                        @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                        @"([\r\n])[\s]+",
                        @"&(quot|#34);",
                        @"&(amp|#38);",
                        @"&(lt|#60);",
                        @"&(gt|#62);", 
                        @"&(nbsp|#160);", 
                        @"&(iexcl|#161);",
                        @"&(cent|#162);",
                        @"&(pound|#163);",
                        @"&(copy|#169);",
                        @"&#(\d+);",
                        @"-->",
                        @"<!--.*\n"
                        };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }

        public string HtmlEncode(string theString)
        {
            //theString = theString.Replace(">", "&gt;");
            //theString = theString.Replace("<", "&lt;");
            //theString = theString.Replace(" ", "&nbsp;");
            //theString = theString.Replace("\"", "&quot;");
            //theString = theString.Replace("\n", "<br/> ");
            //theString = theString.Replace("\r\n", "<br/> ");
            theString = theString.Replace(">", "&gt");
            theString = theString.Replace("<", "&lt");
            theString = theString.Replace(" ", "&nbsp");
            theString = theString.Replace("\"", "&quot");
            theString = theString.Replace("\n", "<br/> ");
            theString = theString.Replace("\r\n", "<br/> ");
            return theString;
        }

        public string HtmlDecode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&amp;", "&");
            //theString = theString.Replace("<br /> ", "\n");
            //theString = theString.Replace("<br/> ", "\n");
            theString = theString.Replace("<br /> ", "");
            theString = theString.Replace("<br/> ", "");
            return theString;
        }
    }
}
