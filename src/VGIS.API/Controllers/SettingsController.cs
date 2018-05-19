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
        private StorageValues _storageSettings;
        private readonly BlobStorageService _blobStorageService;


        public SettingsController(IHostingEnvironment hostingEnvironment, IOptions<StorageValues> storageValues)
        {
            _hostingEnvironment = hostingEnvironment;
            _storageSettings = storageValues.Value;

            _blobStorageService = new BlobStorageService(_storageSettings.StorageAccountCs, _storageSettings.ContainerName);
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
                await data.Avatar.CopyToAsync(stream);
                file = stream.ToArray();
                //var validate = Encoding.ASCII.GetString(stream.ToArray());
            }

            if (file != null)
            {
                await _blobStorageService.UploadFileToBlob(file, data.FileName);
            }

            return Ok();
        }

    }

    public class SettingsData
    {
        public string User { get; set; }
        public string FileName { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
