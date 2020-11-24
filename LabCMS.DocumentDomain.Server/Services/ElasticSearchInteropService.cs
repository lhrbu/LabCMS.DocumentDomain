using LabCMS.DocumentDomain.Shared.Models;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Server.Services
{
    public class ElasticSearchInteropService
    {
        private readonly ElasticClient _client;
        public string IndexName { get; }
        public ElasticSearchInteropService(
            IConfiguration configuration)
        {
            _client = new(new Uri(configuration.GetConnectionString("ElasticSearchUrl")));
            IndexName = configuration["DocumentRepositoryIndexName"] ?? "labcms-document";
            if (!ValidateIndex())
            { throw new InvalidOperationException($"Index: {IndexName} was not created in ES!"); }
        }
        public async Task IndexAsync(Document document) =>
            await _client.IndexAsync(document, i => i.Index(IndexName));

        public async Task DeleteByIdAsync(Guid id) =>
            await _client.DeleteAsync<Document>(id, i => i.Index(IndexName));
        private bool ValidateIndex() => _client.Indices.Exists(IndexName).Exists;
    }
}
