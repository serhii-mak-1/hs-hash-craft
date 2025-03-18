namespace HashCraft.Storage.Services.Exceptions
{
    public class SaveDataException : Exception
    {
        public SaveDataException(Exception ex) 
            : base ("Unable to save data to DB. See inner exception for more details", ex)
        {
        }
    }
}
