﻿using System;
using System.Web.Mvc;

namespace Library.GoogleMap
{
    public static class HtmlHelperExtension
    {
        public static GoogleMapBuilder GoogleMap(this HtmlHelper helper)
        {
            if (helper == null) throw new ArgumentNullException("helper");
            
            return new GoogleMapBuilder(new GoogleMap(helper.ViewContext));
        }
    }
}