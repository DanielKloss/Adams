using TheClockEnd.Models;

namespace TheClockEnd.Data
{
    public interface IDataReaderFactory
    {
        ICustomDataReader Create();
    }
}
