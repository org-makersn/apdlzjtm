﻿using System.Collections.Generic;

namespace Library.GoogleMap
{
    public class ImageMapTypeSerializer : MapTypeSerializer
    {
        private readonly ImageMapType mapType;

        public ImageMapTypeSerializer(ImageMapType mapType) : base(mapType)
        {
            this.mapType = mapType;
        }

        public override IDictionary<string, object> Serialize()
        {
            IDictionary<string, object> result = base.Serialize();
           
            result["repeatHorizontally"] = mapType.RepeatHorizontally;
            result["repeatVertically"] = mapType.RepeatVertically;
            result["tileSize"] = mapType.TileSize;
            result["tileUrlPattern"] = mapType.TileUrlPattern;

            return result;
        }
    }
}