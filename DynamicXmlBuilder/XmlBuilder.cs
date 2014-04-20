namespace DynamicXmlBuilder
{
    public static class XmlBuilder
    {
        public static dynamic Create()
        {
            return new ChildXmlNodesBuilder();
        }
    }
}
