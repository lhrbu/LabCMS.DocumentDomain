using LabCMS.DocumentDomain.Server.Repositories;
using LabCMS.DocumentDomain.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentIndiceRepository _indiceRepository;
        private readonly DocumentContentsRepository _contentsRepository;

        public DocumentController(
            DocumentIndiceRepository indiceRepository,
            DocumentContentsRepository contentsRepository)
        {
            _indiceRepository = indiceRepository;
            _contentsRepository = contentsRepository;
        }

        [HttpPost]
        public async ValueTask PostAsync(Document document)
        {
            _contentsRepository.BeginUploadFile(document.Content);
            await _indiceRepository.InsertAsync(document.Index);
        }
    }
}
