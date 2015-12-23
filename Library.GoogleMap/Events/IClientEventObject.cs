namespace Library.GoogleMap
{
    public interface IClientEventObject
    {
        void SerializeTo(ClientSideObjectWriter writer);
    }
}