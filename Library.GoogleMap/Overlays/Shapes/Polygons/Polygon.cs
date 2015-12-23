﻿using System.Collections.Generic;

namespace Library.GoogleMap
{
    public class Polygon : Shape
    {
        public Polygon(GoogleMap map): base(map)
        {
            
        }

        private List<Location> points;
        public IList<Location> Points
        {
            get
            {
                if (points == null)
                {
                    points = new List<Location>();
                }
                return points.AsReadOnly();
            }
        }

        public virtual void AddPoint(Location point)
        {
            if (points == null)
            {
                points = new List<Location>();
            }
            points.Add(point);
        }

        public override ISerializer CreateSerializer()
        {
            return new PolygonSerializer(this);
        }
    }
}