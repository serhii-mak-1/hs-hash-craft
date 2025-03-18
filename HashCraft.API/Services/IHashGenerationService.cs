using System.Threading;
using System.Threading.Tasks;

namespace HashCraft.API.Services
{
    public interface IHashGenerationService
    {
        Task GenerateAsync(CancellationToken ct = default);
    }
}
