namespace HashCraft.Storage.DAL.Entities
{
    public class Hash
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string Sha1 { get; set; } = string.Empty;
    }
}
