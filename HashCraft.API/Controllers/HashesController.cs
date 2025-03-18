using HashCraft.API.Dto;
using HashCraft.API.Services;
using HashCraft.Storage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HashCraft.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashesController : ControllerBase
    {
        private readonly ILogger<HashesController> _logger;
        private readonly IHashGenerationService _hashGenerationService;
        private readonly HashStorageService _hashStorageService;

        public HashesController(
            ILogger<HashesController> logger,
            IHashGenerationService hashGenerationService,
            HashStorageService hashStorageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hashGenerationService = hashGenerationService ?? throw new ArgumentNullException(nameof(hashGenerationService));
            _hashStorageService = hashStorageService ?? throw new ArgumentNullException(nameof(hashStorageService));
        }

        [HttpPost]
        public async Task GenerateHash(VoidRequest request, CancellationToken ct)
        {
            _logger.LogDebug("Starting hash generation");

            await _hashGenerationService.GenerateAsync(ct);

            _logger.LogInformation("Successfully generated hashes");
        }

        [HttpGet]
        [Route("List")]
        public GetHashesResponse GetList(int take = 100, int skip = 0)
        {
            _logger.LogDebug("Starting fetching hashes");

            var hashes = _hashStorageService.GetHashes(take, skip);

            _logger.LogInformation("Successfully fetched hashes");

            return new GetHashesResponse()
            {
                Hashes = hashes
            };
        }

        [HttpGet]
        public GetHashStatsResponse Get(int take = 10, int skip = 0)
        {
            _logger.LogDebug("Starting fetching hash statistics");

            var stats = _hashStorageService.GetStatistics(take, skip);

            _logger.LogInformation("Successfully fetched hash statistics");

            return new GetHashStatsResponse()
            {
                Hashes = stats
            };
        }
    }
}
