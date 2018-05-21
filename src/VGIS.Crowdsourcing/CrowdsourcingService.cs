using System;
using System.Threading.Tasks;
using VGIS.Crowdsourcing.DAL;
using VGIS.Crowdsourcing.Tools;

namespace VGIS.Crowdsourcing
{
    public interface ICrowdsourcingService
    {
        Task SendSettingsToCloud(string gameId, string pathToSettingsFile);
    }

    public class CrowdsourcingService : ICrowdsourcingService
    {
        private readonly ICrowdsourcingApiDal _crowdsourcingService;
        private readonly IHidGenerator _hidGenerator;
        private readonly IFileHandler _fileHandler;

        public CrowdsourcingService(ICrowdsourcingApiDal crowdsourcingService, IHidGenerator hidGenerator, IFileHandler fileHandler)
        {
            _crowdsourcingService = crowdsourcingService;
            _hidGenerator = hidGenerator;
            _fileHandler = fileHandler;
        }

        public async Task SendSettingsToCloud(string gameId, string pathToSettingsFile)
        {
            var hid = _hidGenerator.GetHid();
            var fileName = _fileHandler.GetFileName(pathToSettingsFile);
            var fileBytes = _fileHandler.GetFileBytes(pathToSettingsFile);
            await _crowdsourcingService.Upload(hid, gameId, fileName, fileBytes);
        }
    }
}
