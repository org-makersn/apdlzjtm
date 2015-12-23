using System.Collections.Generic;

namespace Library.GoogleMap
{
    public interface ISerializer
    {
        IDictionary<string, object> Serialize();
    }
}
