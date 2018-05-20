using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Vgis_crowdsourcing_api.Services;
using Vgis_crowdsourcing_api.Settings;

namespace Vgis_crowdsourcing_api.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly BlobStorageService _blobStorageService;
        private readonly TableStorageService _tableStorageService;
        private readonly StorageValues _storageSettings;


        public SettingsController(IHostingEnvironment hostingEnvironment, IOptions<StorageValues> storageValues)
        {
            _hostingEnvironment = hostingEnvironment;
            _storageSettings = storageValues.Value;


            _blobStorageService = new BlobStorageService(_storageSettings.StorageAccountCs, _storageSettings.ContainerName);
            _tableStorageService = new TableStorageService(_storageSettings.StorageAccountCs, _storageSettings.TableName);
        }


        [HttpGet]
        public string Get()
        {
            return _storageSettings.ContainerName;
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]SettingsData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] file;
            using (var stream = new MemoryStream())
            {
                await data.File.CopyToAsync(stream);
                file = stream.ToArray();
            }

            if (file != null)
            {
                var fileName = await _blobStorageService.UploadFileToBlob(file, $"{data.GameId}.json");
                await _tableStorageService.AddEntryToTable(data.UserId, data.GameId, fileName);
            }

            return Ok();
        }

    }

    public class SettingsData
    {
        public string UserId { get; set; }
        public string GameId { get; set; }
        public IFormFile File { get; set; }
    }
}
