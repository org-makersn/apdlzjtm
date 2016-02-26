using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Web.Front.Helper
{
    public class InputCheckHelper
    {
        public bool inputCheck(string text)
        {
            bool result = true;

            text = text.ToLower();

            if ((text.Contains("delete") && text.Contains("from")) ||
                (text.Contains("select") && text.Contains("from")) ||
                (text.Contains("drop") && text.Contains("table")) ||
                (text.Contains("drop") && text.Contains("function")) ||
                (text.Contains("from") && text.Contains("where")) ||
                (text.Contains("sys.")) ||
                (text.Contains("insert") && text.Contains("into")) ||
                (text.Contains("update") && text.Contains("set")) ||
                (text.Contains("update") && text.Contains("where")) ||
                (text.Contains("drop") && text.Contains("db")) ||
                (text.Contains("drop") && text.Contains("colume")) ||
                (text.Contains("create") && text.Contains("db")) ||
                (text.Contains("create") && text.Contains("table")) ||
                (text.Contains("case") && text.Contains("when")) ||
                (text.Contains("drop") && text.Contains("--")) ||
                (text.Contains("<script>"))||
                (text.Contains("master.")) ||
                (text.Contains("use") && text.Contains("db")))
            {
                result = false;
            }

            

            return result;
        }
    }
}