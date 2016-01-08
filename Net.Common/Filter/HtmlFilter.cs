using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Net.Common.Filter
{
    public class HtmlFilter
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string HtmlEncoding(string contents)
        {
            return WebUtility.HtmlEncode(contents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string HtmlDecoding(string contents)
        {
            return WebUtility.HtmlDecode(contents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public string Unused_HtmlToTxt(string strHtml)
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

        ///
        public string PunctuationEncode(string str)
        {
            str = str.Replace(">", "&gt;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("\"", "&quot;");
            str = str.Replace("\n", "<br/> ");
            str = str.Replace("\r\n", "<br/> ");
            return str;
        }

        public string PunctuationDecode(string str)
        {
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&amp;", "&");
            //theString = theString.Replace("<br /> ", "\n");
            //theString = theString.Replace("<br/> ", "\n");
            str = str.Replace("<br /> ", "");
            str = str.Replace("<br/> ", "");
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string ConvertContent(string contents)
        {
            //프로토콜부분 - 있을수도 없을수도
            //string ptProtocol = "(?:(ftp|https?|mailto|telnet):\\/\\/)?";
            string ptProtocol = "(?:(ftp|https?|mailto|telnet):\\/\\/)";
            //domain의 기본 골격은 daum.net
            string domain = @"[a-zA-Z]\w+\.[a-zA-Z]\w+(\.\w+)?(\.\w+)?";
            //도메인 뒤에 추가로 붙는 서브url 및 파라미터들
            //이부분이 아직은 미흡하여 오류가 가끔 일어난다.
            string adds = "([:?=&/%.-]+\\w+)*";
            Regex rgxDomain = new Regex(ptProtocol + domain + adds, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchDomain = rgxDomain.Match(contents);

            Regex rgxEmail = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchEmail = rgxEmail.Match(contents);

            string[] emailList = new string[matchEmail.Length];
            int index = 0;
            while (matchEmail.Success)
            {
                if (Regex.IsMatch(matchEmail.Value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    emailList[index] = "<a href='mailto:" + matchEmail.Value + "'>" + matchEmail.Value + "</a>";
                    contents = contents.Replace(matchEmail.Value, "#" + index + "#");
                    index++;
                }
                //contents = contents.Replace(matchEmail.Value, "<a href='mailto:" + matchEmail.Value + "'>" + matchEmail.Value + "</a>");
                matchEmail = matchEmail.NextMatch();
            }

            //string domainList = string.Empty;
            //while (matchDomain.Success)
            //{
            //    if (!domainList.Contains(matchDomain.Value))
            //    {
            //        contents = contents.Replace(matchDomain.Value, "<a href='" + matchDomain.Value + "' target='_blank'>" + matchDomain.Value + "</a>");
            //        domainList += matchDomain.Value;
            //        matchDomain = matchDomain.NextMatch();
            //    }
            //    else
            //    {
            //        matchDomain = matchDomain.NextMatch();
            //    }
            //}

            #region
            //string domainList = string.Empty;
            IList<string> domainList = new List<string>();
            contents += "&nbsp";
            while (matchDomain.Success)
            {
                //if (!domainList.Contains(matchDomain.Value))
                //{
                //    //contents = contents.Replace(matchDomain.Value, "<a href='" + matchDomain.Value + "' target='_blank'>" + matchDomain.Value + "</a>");
                //    //domainList += matchDomain.Value;
                //    contents = contents.Replace(matchDomain.Value.Replace("&nbsp", ""), "<a href='" + matchDomain.Value.Replace("&nbsp", "") + "' target='_blank'>" + matchDomain.Value.Replace("&nbsp","") + "</a>");
                //    domainList.Add(matchDomain.Value);
                //    matchDomain = matchDomain.NextMatch();
                //}
                //else
                //{
                //    matchDomain = matchDomain.NextMatch();
                //}

                string replaceDomain = matchDomain.Value.Replace("&nbsp", "");
                if (!domainList.Contains(replaceDomain))
                {
                    //contents = contents.Replace(matchDomain.Value, "<a href='" + matchDomain.Value + "' target='_blank'>" + matchDomain.Value + "</a>");
                    //domainList += matchDomain.Value;
                    contents = contents.Replace(replaceDomain + "&nbsp", "<a href='" + replaceDomain + "' target='_blank'>" + replaceDomain + "</a>");
                    contents = contents.Replace(replaceDomain + "\r", "<a href='" + replaceDomain + "' target='_blank'>" + replaceDomain + "</a>");

                    domainList.Add(replaceDomain);
                    matchDomain = matchDomain.NextMatch();
                }
                else
                {
                    matchDomain = matchDomain.NextMatch();
                }


            }
            #endregion

            for (int i = 0; i < emailList.Length; i++)
            {
                contents = contents.Replace("#" + i + "#", emailList[i]);
            }

            return contents;
        }
    }
}