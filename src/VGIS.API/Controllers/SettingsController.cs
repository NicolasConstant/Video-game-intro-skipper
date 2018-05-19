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
using Vgis_crowdsourcing_api.Settings;

namespace Vgis_crowdsourcing_api.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private StorageValues _storageSettings;

        public SettingsController(IHostingEnvironment hostingEnvironment, IOptions<StorageValues> storageValues)
        {
            _hostingEnvironment = hostingEnvironment;
            _storageSettings = storageValues.Value;
        }


        [HttpGet]
        public string Get()
        {
            return _storageSettings.ContainerName;
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]User vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"Uploads", vm.Avatar.FileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            using (var stream = new MemoryStream())
            {
                await vm.Avatar.CopyToAsync(stream);
                if (stream.CanRead)
                {
                    var fileBytes = stream.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                }

            }

            return Ok();
        }

    }

    public class User
    {
        public string Name { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
