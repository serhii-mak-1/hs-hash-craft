using HashCraft.API.Configuration;
using HashCraft.API.Tools;
using HashCraft.API.Tools.Sha1HashGenerator;
using HashCraft.Infrastructure.MessageServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HashCraft.API.Services.HashGenerationService
{
    public class HashGenerationService : IHashGenerationService
    {
        private readonly IPublisherFactory _publisherFactory;
        private readonly ILogger<HashGenerationService> _logger;
        private readonly HashCraftApiConfig _config;
        private readonly IHashGenerator _hashGenerator;

        public HashGenerationService(
            IPublisherFactory publisherFactory,
            ILogger<HashGenerationService> logger,
            IOptions<HashCraftApiConfig> configOptions,
            IHashGenerator hashGenerator)
        {
            _publisherFactory = publisherFactory ?? throw new ArgumentNullException(nameof(publisherFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = configOptions.Value ?? throw new ArgumentNullException(nameof(configOptions));
            _hashGenerator = hashGenerator ?? throw new ArgumentNullException(nameof(hashGenerator));
        }

        public async Task GenerateAsync(CancellationToken ct = default)
        {
            var tasks = new List<Task>();

            using var publisher = await _publisherFactory.CreateAsync(ct);

            for (int i = 0; i < _config.Total / _config.BatchSize; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    string[] hashes = _hashGenerator.GenerateHashBatch(_config.BatchSize, GetEncodeStyle());
                    await publisher.PublishAsync("hash_batch", hashes, ct);
                }, ct));
            }

            await Task.WhenAll(tasks);
        }

        private EncodeStyle GetEncodeStyle()
        {
            if (!Enum.TryParse(_config.EncodeStyle, true, out EncodeStyle encode))
            {
                throw new FormatException();
            }

            return encode;
        }
    }
}
