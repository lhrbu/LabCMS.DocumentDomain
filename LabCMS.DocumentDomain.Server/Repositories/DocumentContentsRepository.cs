using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabCMS.DocumentDomain.Shared.Models;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Configuration;


namespace LabCMS.DocumentDomain.Server.Repositories
{
    public class DocumentContentsRepository:IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILiteDatabaseAsync _database;
        private readonly ILiteStorageAsync<string> _fileStorage;
        public DocumentContentsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _database = new LiteDatabaseAsync(_configuration.GetConnectionString(nameof(DocumentContentsRepository)));
            _fileStorage = _database.FileStorage;
        }

        public async Task UploadFileAsync(DocumentContent documentContent)
        {
            using MemoryStream stream = new(Convert.FromBase64String(documentContent.Base64Content));
            string idString = documentContent.Id.ToString();
            await _fileStorage.UploadAsync(idString, idString, stream);
        }

        public void BeginUploadFile(DocumentContent documentContent) =>
            UploadFileAsync(documentContent).ConfigureAwait(false);

        public async ValueTask<DocumentContent> FindByIdAsync(Guid id)
        {
            LiteFileInfo<string> liteFileInfo =await _fileStorage.FindByIdAsync(id.ToString());
            byte[] content = new byte[liteFileInfo.Length];
            using MemoryStream stream = new(content);
            liteFileInfo.CopyTo(stream);
            return new DocumentContent()
            {
                Id = id,
                Base64Content = Convert.ToBase64String(content)
            };
        }

        public async ValueTask DeleteByIdAsync(Guid id) =>
            await _fileStorage.DeleteAsync(id.ToString());

        public void Dispose()=>_database.Dispose();
        
    }
}
