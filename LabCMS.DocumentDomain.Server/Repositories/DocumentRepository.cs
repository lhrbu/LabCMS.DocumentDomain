using LabCMS.DocumentDomain.Server.Services;
using LabCMS.DocumentDomain.Shared.Models;
using LiteDB.Async;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace LabCMS.DocumentDomain.Server.Repositories
{
    public class DocumentRepository:IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILiteDatabaseAsync _database;
        private readonly ILiteStorageAsync<Guid> _fileStorage;
        private readonly ElasticSearchInteropService _elasticSearch;

        public DocumentRepository(
            IConfiguration configuration,
            ElasticSearchInteropService elasticSearch)
        {
            _configuration = configuration;
            _elasticSearch = elasticSearch;

            _database = new LiteDatabaseAsync(_configuration.GetConnectionString(nameof(DocumentRepository)));
            _fileStorage = _database.GetStorage<Guid>();
        }

        public async ValueTask UploadAsync(Document document)
        {
            using MemoryStream stream = new();
            await JsonSerializer.SerializeAsync(stream, document);
            stream.Seek(0, SeekOrigin.Begin);
            _=_elasticSearch.IndexAsync(document).ConfigureAwait(false);
            await _fileStorage.UploadAsync(document.Id, document.Title,stream);
        }

        public async ValueTask DeleteByIdAsync(Guid id)
        {
            _=_elasticSearch.DeleteByIdAsync(id).ConfigureAwait(false);
            await _fileStorage.DeleteAsync(id);
        }









        public void Dispose() => _database.Dispose();
    }
}
