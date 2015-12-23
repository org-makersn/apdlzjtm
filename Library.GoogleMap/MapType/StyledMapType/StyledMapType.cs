using System.Collections.Generic;

namespace Library.GoogleMap
{
    public class StyledMapType : MapType
    {
        public StyledMapType()
        {
            Styles = new List<MapTypeStyle>();
        }

        public IList<MapTypeStyle> Styles { get; private set; }

        public override ISerializer CreateSerializer()
        {
            return new StyledMapTypeSerializer(this);
        }
    }
}