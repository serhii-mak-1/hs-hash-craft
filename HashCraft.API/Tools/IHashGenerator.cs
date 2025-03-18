using HashCraft.API.Tools.Sha1HashGenerator;

namespace HashCraft.API.Tools
{
    public interface IHashGenerator
    {
        string GenerateHash(EncodeStyle encodeStyle);

        string[] GenerateHashBatch(int size, EncodeStyle encodeStyle);
    }
}
