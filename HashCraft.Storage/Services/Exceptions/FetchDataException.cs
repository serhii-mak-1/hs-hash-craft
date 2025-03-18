namespace HashCraft.Storage.Services.Exceptions
{
    public class FetchDataException : Exception
    {
        public FetchDataException(Exception ex)
            : base ("Error during fetching data from database. For more details see inner exception", ex)
        {
        }
    }
}
