using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Web.Front.Models
{
    public class LanguageModel
    {
        public virtual string SiteLanguage { get; set; }
        public virtual string SiteLanguageName { get; set; }
        public virtual string PostsLanguage { get; set; }
        public virtual string PostsLanguageName { get; set; }
    }
}