namespace Library.GoogleMap
{
    public enum MapTypes
    {
        [ClientSideEnumValue("'HYBRID'")]
        Hybrid,
        [ClientSideEnumValue("'ROADMAP'")]
        Roadmap,
        [ClientSideEnumValue("'SATELLITE'")]
        Satellite,
        [ClientSideEnumValue("'TERRAIN'")]
        Terrain
    }
}
