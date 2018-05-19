using System.IO;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Domain
{
    public class GameDetectionResult
    {
        #region Ctor
        public GameDetectionResult(string name)
        {
            GameName = name;
        }
        #endregion

        public string GameName { get; }
        public bool Detected { get; set; }
        public IntroductionStateEnum IntroductionState { get; set; }
        public DirectoryInfo InstallationPath { get; set; }
    }
}