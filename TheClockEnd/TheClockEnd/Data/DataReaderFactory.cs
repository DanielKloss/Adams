using System;

namespace TheClockEnd.Data
{
    public class DataReaderFactory
    {
        public static ICustomDataReader GetDataReader(string data)
        {
            ICustomDataReader reader = null;

            switch (DataType.XML)
            {
                case DataType.XML:
                    reader = new XmlDataReader(data + ".xml");
                    break;
                default:
                    throw new Exception("Invalid Data Reader Type Provided");
            }

            return reader;
        }
    }
}
