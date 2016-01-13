namespace TheClockEnd.Data
{
    public class XmlDataReaderFactory : IDataReaderFactory
    {
        public ICustomDataReader Create()
        {
            return new XmlDataReader();
        }
    }
}
