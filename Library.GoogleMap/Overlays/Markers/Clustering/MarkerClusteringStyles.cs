﻿using System.Drawing;

namespace Library.GoogleMap
{
    public class MarkerClusteringStyles
    {
        private string url;

        public string Url
        {
            get { return url; }
            set { url = System.Web.VirtualPathUtility.ToAbsolute(value); }
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public int TextSize { get; set; }
        public Color TextColor { get; set; }

        public ISerializer CreateSerializer()
        {
            return new MarkerClusteringStylesSerializer(this);
        }
    }
}