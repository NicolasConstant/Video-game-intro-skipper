using System.IO;

namespace VGIS.Domain.Domain
{
    public class GameDetectionResult
    {
        public int GameId { get; set; } //TODO 
        public string GameName { get; set; }
        public bool Detected { get; set; }
        public bool IsIntroductionDisabled { get; set; }
        public DirectoryInfo InstallationPath { get; set; }
    }
}