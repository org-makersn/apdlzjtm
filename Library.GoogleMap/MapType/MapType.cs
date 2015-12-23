﻿namespace Library.GoogleMap
{
    public abstract class MapType : IHideObjectMembers
    {
        protected MapType()
        {
            Radius = 6378137;
            Opacity = 100;
        }

        public string MapTypeAltName { get; set; }
        public int MaxZoom { get; set; }
        public int MinZoom { get; set; }
        public string MapTypeName { get; set; }
        public int Opacity { get; set; }
        public int Radius { get; set; }

        public abstract ISerializer CreateSerializer();
    }
}