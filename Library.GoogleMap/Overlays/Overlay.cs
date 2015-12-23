using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GoogleMap
{
    public abstract class Overlay
    {
        private readonly GoogleMap map;
        private double? longitude;
        private double? latitude;

        protected Overlay(GoogleMap map)
        {
            if (map == null) throw new ArgumentNullException("map");
            this.map = map;
        }

        protected internal GoogleMap Map
        {
            get { return this.map; }
        }
        public virtual double? Longitude
        {
            get
            {
                return longitude;
            }
            set { longitude = value; }
        }

        public virtual double? Latitude
        {
            get
            {
                return latitude;
            }
            set { latitude = value; }
        }
    }
}