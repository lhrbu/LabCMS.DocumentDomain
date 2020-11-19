using LabCMS.DocumentDomain.Shared.Models;
using LiteDB;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB.Async;
using Nest;

namespace LabCMS.DocumentDomain.Server.Repositories
{
    public class DocumentIndiceRepository:IDisposable
    {
        private readonly IConfiguration _configuration;

        private readonly ILiteDatabaseAsync _database;
        private readonly ILiteCollectionAsync<DocumentIndex> _documentIndices;
        private readonly IElasticClient _elasticClient;
        
        public DocumentIndiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _database = new LiteDatabaseAsync(_configuration.GetConnectionString(nameof(DocumentIndiceRepository)));
            _documentIndices = _database.GetCollection<DocumentIndex>();
            _documentIndices.EnsureIndexAsync(item => item.Tags).Wait();
            _documentIndices.EnsureIndexAsync(item => item.Levels).Wait();
            _elasticClient = new ElasticClient(new Uri(_configuration.GetConnectionString("ElasticSearchUrl")));
            TryCreateIndex();

        }

        private bool TryCreateIndex()
        {
            string indexName = nameof(DocumentIndex).ToLower();
            if (!_elasticClient.Indices.Exists(indexName).Exists)
            {
                _elasticClient.Indices.Create(indexName, builder => builder.Map<DocumentIndex>(
                    mapper => mapper.AutoMap()
                        //.Properties(item => item
                        //        .Date(d => d
                        //            .Name(e => e.PublishedDate).Format("yyyy/MM/dd")))
                     ));
                return true;
            }
            else { return false; }
        }

        private async Task<IndexResponse> InsertIntoElasticSearchAsync(DocumentIndex documentIndex)
        {
            var res = await _elasticClient.IndexAsync(documentIndex, 
                item => item.Index(nameof(DocumentIndex).ToLower()));
            return res;
        }
        public async ValueTask InsertAsync(DocumentIndex documentIndex)
        {
            _=InsertIntoElasticSearchAsync(documentIndex)
                .ConfigureAwait(false);
            await _documentIndices.InsertAsync(documentIndex);
        }

        public async ValueTask DeleteByIdAsync(Guid id)
        {
            _=_elasticClient.DeleteAsync(new DocumentPath<DocumentIndex>(id)).ConfigureAwait(false);
            await _documentIndices.DeleteAsync(id); 
        }
        
        public async ValueTask<IEnumerable<DocumentIndex>> GetAll() => await _documentIndices.FindAllAsync();
        public async ValueTask<DocumentIndex> GetById(Guid id) => await _documentIndices.FindByIdAsync(id);

        public void Dispose()=>_database.Dispose();
        
    }
}
